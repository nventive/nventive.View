#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Uno.Disposables;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Nventive.View.Controls
{
	/// <summary>
	/// The goal of the control is to delay view creation.
	/// To do so, either set DelayAfterLoad or DelayAfterDataContext or both.
	/// The view creation will be scheduled after the Loaded event or when the DataContext is received.
	/// Setting both delay properties will wait for both the Loaded event and the DataContext before scheduling with the longest time from the two.
	/// </summary>
	[TemplateVisualState(Name = WaitingVisualStateName, GroupName = "WaitingStates")]
	[TemplateVisualState(Name = ReadyVisualStateName, GroupName = "WaitingStates")]
	[TemplatePart(Name = ContentPresenterPartName, Type = typeof(ContentPresenter))]
	[ContentProperty(Name = "ContentTemplate")]
	public partial class DelayControl : Control
	{
		private const string WaitingVisualStateName = "Waiting";
		private const string ReadyVisualStateName = "Ready";
		private const string ContentPresenterPartName = "PART_ContentPresenter";

		private ContentPresenter _contentPresenter;
		private SerialDisposable _delayDisposable = new SerialDisposable();
		private bool _hasSetContentTemplate = false;
		private bool _hasMaterializedView = false;
		private bool _isLoaded = false;
		private bool _isTemplated = false;
		private bool _hasDataContext = false;
		private bool _isContextSetBeforeView = false;
		private IScheduler _dispatcher;

		/// <summary>
		/// This is the template containing the custom view that we wait to delay.
		/// </summary>
		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ContentTemplateProperty =
			DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(DelayControl), new PropertyMetadata(null));

		/// <summary>
		/// This is the materizalied view. You can use this in Xaml when you need to bind with ElementName.
		/// </summary>
		public object InnerView
		{
			get { return (object)GetValue(InnerViewProperty); }
			set { SetValue(InnerViewProperty, value); }
		}

		public static readonly DependencyProperty InnerViewProperty =
			DependencyProperty.Register("InnerView", typeof(object), typeof(DelayControl), new PropertyMetadata(null));

		/// <summary>
		/// Set this property to schedule the view creation after the Loaded event.
		/// </summary>
		public TimeSpan DelayAfterLoad
		{
			get { return (TimeSpan)GetValue(DelayAfterLoadProperty); }
			set { SetValue(DelayAfterLoadProperty, value); }
		}

		public static readonly DependencyProperty DelayAfterLoadProperty =
			DependencyProperty.Register("DelayAfterLoad", typeof(TimeSpan), typeof(DelayControl), new PropertyMetadata(TimeSpan.Zero));

		/// <summary>
		/// Set this property to schedule the view creation once a DataContext is received.
		/// </summary>
		public TimeSpan DelayAfterDataContext
		{
			get { return (TimeSpan)GetValue(DelayAfterDataContextProperty); }
			set { SetValue(DelayAfterDataContextProperty, value); }
		}

		public static readonly DependencyProperty DelayAfterDataContextProperty =
			DependencyProperty.Register("DelayAfterDataContext", typeof(TimeSpan), typeof(DelayControl), new PropertyMetadata(TimeSpan.Zero));

		/// <summary>
		/// Set this property to schedule the view creation once <see cref="IsAllowedToMaterialize"/> is set to true.
		/// </summary>
		public TimeSpan DelayAfterManualTrigger
		{
			get { return (TimeSpan)GetValue(DelayAfterManualTriggerProperty); }
			set { SetValue(DelayAfterManualTriggerProperty, value); }
		}

		public static readonly DependencyProperty DelayAfterManualTriggerProperty =
			DependencyProperty.Register("DelayAfterManualTrigger", typeof(TimeSpan), typeof(DelayControl), new PropertyMetadata(TimeSpan.Zero));

		/// <summary>
		/// The priority used when we apply the ContentTemplate on the PART_ContentPresenter.
		/// </summary>
		public CoreDispatcherPriority ApplyTemplatePriority
		{
			get { return (CoreDispatcherPriority)GetValue(ApplyTemplatePriorityProperty); }
			set { SetValue(ApplyTemplatePriorityProperty, value); }
		}

		public static readonly DependencyProperty ApplyTemplatePriorityProperty =
			DependencyProperty.Register("ApplyTemplatePriority", typeof(CoreDispatcherPriority), typeof(DelayControl), new PropertyMetadata(CoreDispatcherPriority.Normal));

		/// <summary>
		/// The priority used when resolving the InnerView.
		/// </summary>
		public CoreDispatcherPriority InnerViewPriority
		{
			get { return (CoreDispatcherPriority)GetValue(InnerViewPriorityProperty); }
			set { SetValue(InnerViewPriorityProperty, value); }
		}

		public static readonly DependencyProperty InnerViewPriorityProperty =
			DependencyProperty.Register("InnerViewPriority", typeof(CoreDispatcherPriority), typeof(DelayControl), new PropertyMetadata(CoreDispatcherPriority.Normal));

		/// <summary>
		/// When true, the visual state will be changed only once the WaitingControl has a reference on the materialized view from its ContentTemplate.
		/// </summary>
		public bool WaitForInnerView
		{
			get { return (bool)GetValue(WaitForInnerViewProperty); }
			set { SetValue(WaitForInnerViewProperty, value); }
		}

		public static readonly DependencyProperty WaitForInnerViewProperty =
			DependencyProperty.Register("WaitForInnerView", typeof(bool), typeof(DelayControl), new PropertyMetadata(false));

		/// <summary>
		/// You can use this along with <see cref="DelayAfterManualTrigger"/> to manually control when the view will be materialized.
		/// The default value is true so that you can use the other delay properties as default behavior.
		/// </summary>
		public bool IsAllowedToMaterialize
		{
			get { return (bool)GetValue(IsAllowedToMaterializeProperty); }
			set { SetValue(IsAllowedToMaterializeProperty, value); }
		}

		public static readonly DependencyProperty IsAllowedToMaterializeProperty =
			DependencyProperty.Register("IsAllowedToMaterialize", typeof(bool), typeof(DelayControl), new PropertyMetadata(true, OnIsAllowedToMaterializeChanged));

		private static void OnIsAllowedToMaterializeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var isAllowedToMaterialize = (bool)args.NewValue;
			if (isAllowedToMaterialize)
			{
				var that = (DelayControl)dependencyObject;
				var delay = that.DelayAfterManualTrigger;
				if (delay == TimeSpan.Zero)
				{
					// We don't want Zero when using manual, because we want the sourceDelay parameter to have a valid value.
					delay = TimeSpan.FromMilliseconds(1);
				}

				that.TryMaterialize(delay);
			}
		}

		public DelayControl()
		{
			DefaultStyleKey = typeof(DelayControl);
			Loaded += OnLoaded;
			DataContextChanged += OnDataContextChanged;

			_dispatcher = new MainDispatcherScheduler(Dispatcher);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_contentPresenter = (ContentPresenter)GetTemplateChild(ContentPresenterPartName);

			VisualStateManager.GoToState(this, WaitingVisualStateName, false);

			_isTemplated = true;

			if (_isContextSetBeforeView)
			{
				TryMaterialize(DelayAfterDataContext);
			}
			else
			{
				TryMaterialize(DelayAfterLoad);
			}
		}

		private bool TryMaterialize(TimeSpan sourceDelay)
		{
			if (_hasSetContentTemplate)
			{
				// Already materialized.
				return false;
			}

			if (!_isTemplated)
			{
				// Control not templated yet.
				return false;
			}

			if (!IsAllowedToMaterialize)
			{
				// Control is not allowed to materialize.
				return false;
			}

			var delayAfterLoad = DelayAfterLoad;
			var delayAfterDataContext = DelayAfterDataContext;

			var hasDelayAfterLoad = delayAfterLoad != TimeSpan.Zero;
			var hasDelayAfterDataContext = delayAfterDataContext != TimeSpan.Zero;
			var hasSourceDelay = sourceDelay != TimeSpan.Zero;

			var hasAllDelays = hasDelayAfterLoad && hasDelayAfterDataContext && hasSourceDelay;
			var hasNoDelay = !hasDelayAfterLoad && !hasDelayAfterDataContext && !hasSourceDelay;

			if (hasAllDelays && !_isLoaded && !_hasDataContext)
			{
				// Not ready yet.
				return false;
			}

			if (!hasAllDelays && !hasSourceDelay && !hasNoDelay)
			{
				// If the delay is coming from another source,
				// wait for it to ask for materialziation.
				return false;
			}

			var materializationDelay = TimeSpan.Zero;

			if (hasAllDelays)
			{
				// Take the longest of both.
				materializationDelay = TimeSpan.FromTicks(Math.Max(delayAfterLoad.Ticks, delayAfterDataContext.Ticks));
			}
			else if (hasSourceDelay)
			{
				materializationDelay = sourceDelay;
			}

			ScheduleMaterialize(materializationDelay, ApplyTemplatePriority, InnerViewPriority);

			return true;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;

			if (_isContextSetBeforeView)
			{
				TryMaterialize(DelayAfterDataContext);
			}
			else
			{
				TryMaterialize(DelayAfterLoad);
			}

			Loaded -= OnLoaded;
		}

		private void OnDataContextChanged(object sender, DataContextChangedEventArgs args)
		{
			if (args.NewValue != null)
			{
				_hasDataContext = true;

				TryMaterialize(DelayAfterDataContext);

				if (!_isLoaded || !_isTemplated)
				{
					_isContextSetBeforeView = true;
				}

				DataContextChanged -= OnDataContextChanged;
			}
		}

		private void ScheduleMaterialize(TimeSpan delay, CoreDispatcherPriority applyTemplatePriority, CoreDispatcherPriority innerViewPriority)
		{
			_delayDisposable.Disposable = _dispatcher.ScheduleAsync(delay, async (scheduler, ct) =>
			{
				_hasSetContentTemplate = true;

				await Dispatcher.RunAsync(applyTemplatePriority, () =>
				{
					_contentPresenter.ContentTemplate = this.ContentTemplate;

					if (!WaitForInnerView)
					{
						VisualStateManager.GoToState(this, ReadyVisualStateName, true);
					}
				});

				while (!_hasMaterializedView)
				{
					await Dispatcher.RunAsync(innerViewPriority, () =>
					{
						InnerView = VisualTreeHelper.GetChild(_contentPresenter, 0);
						_hasMaterializedView = InnerView != null;

						if (_hasMaterializedView && WaitForInnerView)
						{
							VisualStateManager.GoToState(this, ReadyVisualStateName, true);
						}
					});

					// This seems to be only necessary on UWP.
					// The View seems to need more scheduler loops before having the child view.
					if (!_hasMaterializedView)
					{
						await Task.Delay(50, ct);
					}
				}
			});
		}
	}
}
#endif
