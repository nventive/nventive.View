#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Uno.Logging;
using Windows.UI.Xaml;
using Windows.Devices.Sensors;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using Uno.Disposables;

namespace Chinook.View.Controls
{
	/// <summary>
	/// Control to display as a popup a membership card when the device is rotated to landscape, in an app which is otherwise locked to portrait.
	/// </summary>
	public partial class MembershipCardControl : Control
	{
		private const string AnimationVisualStateGroup = "Animation";
		private const string OpenVisualState = "Open";
		private const string ClosedVisualState = "Closed";

		private readonly SerialDisposable _subscriptions = new SerialDisposable();

		private bool _isViewReady;
		private bool _isLoaded;
		private CompositeDisposable _innerSubscriptions;

#region Control Properties & Dependency properties

#region Content Dependency Property
		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(MembershipCardControl), new PropertyMetadata(default(object)));
#endregion

#region ContentTemplate Dependency Property
		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ContentTemplateProperty =
			DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(MembershipCardControl), new PropertyMetadata(default(DataTemplate)));
#endregion

#region ContentTemplateSelector Dependency Property
		public DataTemplateSelector ContentTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
			set { SetValue(ContentTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty ContentTemplateSelectorProperty =
			DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(MembershipCardControl), new PropertyMetadata(default(DataTemplateSelector)));
#endregion

#region IsBrightnessSetToMaximum Dependency Property

		public bool IsBrightnessSetToMaximum
		{
			get { return (bool)GetValue(IsBrightnessSetToMaximumProperty); }
			set { SetValue(IsBrightnessSetToMaximumProperty, value); }
		}

		public static readonly DependencyProperty IsBrightnessSetToMaximumProperty =
			DependencyProperty.Register("IsBrightnessSetToMaximum", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(true));

#endregion

#region IsStatusBarHidden Dependency Property

		public bool IsStatusBarHidden
		{
			get { return (bool)GetValue(IsStatusBarHiddenProperty); }
			set { SetValue(IsStatusBarHiddenProperty, value); }
		}

		public static readonly DependencyProperty IsStatusBarHiddenProperty =
			DependencyProperty.Register("IsStatusBarHidden", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(true));

#endregion

#region IsKeyboardHidden Dependency Property

		public bool IsKeyboardHidden
		{
			get { return (bool)GetValue(IsKeyboardHiddenProperty); }
			set { SetValue(IsKeyboardHiddenProperty, value); }
		}

		public static readonly DependencyProperty IsKeyboardHiddenProperty =
			DependencyProperty.Register("IsKeyboardHidden", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(true));

#endregion

#region IsOtherPopupsClosed Dependency Property

		public bool IsOtherPopupsClosed
		{
			get { return (bool)GetValue(IsOtherPopupsClosedProperty); }
			set { SetValue(IsOtherPopupsClosedProperty, value); }
		}

		public static readonly DependencyProperty IsOtherPopupsClosedProperty =
			DependencyProperty.Register("IsOtherPopupsClosed", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(true));

#endregion

		private const double MaximalBrightness = 1;

		public double ScreenHeight
		{
			get { return (double)GetValue(ScreenHeightProperty); }
			set { SetValue(ScreenHeightProperty, value); }
		}

		public static readonly DependencyProperty ScreenHeightProperty =
			DependencyProperty.Register("ScreenHeight", typeof(double), typeof(MembershipCardControl), new PropertyMetadata(default(double)));

		public double ScreenWidth
		{
			get { return (double)GetValue(ScreenWidthProperty); }
			set { SetValue(ScreenWidthProperty, value); }
		}

		public static readonly DependencyProperty ScreenWidthProperty =
			DependencyProperty.Register("ScreenWidth", typeof(double), typeof(MembershipCardControl), new PropertyMetadata(default(double)));

		public bool IsAutoOpenOnLandscape
		{
			get { return (bool)GetValue(IsAutoOpenOnLandscapeProperty); }
			set { SetValue(IsAutoOpenOnLandscapeProperty, value); }
		}

		public static readonly DependencyProperty IsAutoOpenOnLandscapeProperty =
			DependencyProperty.Register("IsAutoOpenOnLandscape", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(true));

#endregion

		public MembershipCardControl()
		{
			DefaultStyleKey = typeof(MembershipCardControl);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			_subscriptions.Disposable = null;

			_isLoaded = false;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			ScreenHeight = Windows.UI.Xaml.Window.Current.Bounds.Height;
			ScreenWidth = Windows.UI.Xaml.Window.Current.Bounds.Width;

			VisualStateManager.GoToState(this, ClosedVisualState, useTransitions: true);

			_isLoaded = true;

			Initialize();
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_isViewReady = true;

			Initialize();
		}

		private void Initialize()
		{
			if (!_isLoaded || !_isViewReady)
			{
				return;
			}

			_innerSubscriptions = new CompositeDisposable();

			SubscribeToOrientation(_innerSubscriptions);
			SubscribeToBackButton(_innerSubscriptions);

			_subscriptions.Disposable = _innerSubscriptions;
		}

		private void SubscribeToBackButton(CompositeDisposable innerSubscriptions)
		{
			var navigationManager = SystemNavigationManager.GetForCurrentView();

			if (navigationManager != null)
			{
				navigationManager.BackRequested += BackRequested;
				innerSubscriptions.Add(() => navigationManager.BackRequested -= BackRequested);
			}
		}

		private void BackRequested(object sender, BackRequestedEventArgs e)
		{
			if (IsOpen)
			{
				IsOpen = false;
				e.Handled = true;
			}
		}

		private void SubscribeToOrientation(CompositeDisposable innerSubscriptions)
		{
			var simpleOrientationSensor = SimpleOrientationSensor.GetDefault();

			if (simpleOrientationSensor != null)
			{
				simpleOrientationSensor.OrientationChanged += OrientationChanged;
				innerSubscriptions.Add(() => simpleOrientationSensor.OrientationChanged -= OrientationChanged);
			}
		}

		private void OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
		{
			try
			{
				var orientation = args.Orientation.IsOneOf(
							SimpleOrientation.Rotated270DegreesCounterclockwise,
							SimpleOrientation.Rotated90DegreesCounterclockwise
						)
						? DeviceOrientation.Landscape
						: DeviceOrientation.Portrait;

				if (IsAutoOpenOnLandscape)
				{
					IsOpen = (orientation == DeviceOrientation.Landscape);
				}
			}
			catch (Exception ex)
			{
				this.Log().ErrorIfEnabled(() => $"Error in OrientationChanged subscription: {ex.ToString()}");
			}
		}

		private void OnOpenChanged(bool isOpen)
		{
			_ = Dispatcher.RunTaskAsync(CoreDispatcherPriority.Normal, async () =>
			{
				await ExecuteStrategies(isOpen);
			});
		}

		public bool IsOpen
		{
			get { return (bool)GetValue(IsOpenProperty); }
			set { SetValue(IsOpenProperty, value); }
		}

		public static readonly DependencyProperty IsOpenProperty =
			DependencyProperty.Register("IsOpen", typeof(bool), typeof(MembershipCardControl), new PropertyMetadata(false, IsOpenPropertyChanged));

		private static void IsOpenPropertyChanged(DependencyObject dObject, DependencyPropertyChangedEventArgs args)
		{
			((MembershipCardControl)dObject).OnOpenChanged((bool)args.NewValue);
		}

		private async Task ExecuteStrategies(bool isOpen)
		{
			try
			{
				var state = isOpen ? OpenVisualState : ClosedVisualState;
				VisualStateManager.GoToState(this, state, useTransitions: true);

#if __ANDROID__ || __IOS__
				if (IsBrightnessSetToMaximum)
				{
					if (isOpen)
					{
						SetBrightness(MaximalBrightness);
					}
					else
					{
						ResetBrightness();
					}
				}

				if (IsStatusBarHidden)
				{
					if (isOpen)
					{
						await StatusBar.GetForCurrentView().HideAsync();
					}
					else
					{
						await StatusBar.GetForCurrentView().ShowAsync();
					}
				}
#endif
				if (IsKeyboardHidden && isOpen)
				{
					InputPane.GetForCurrentView().TryHide();
#if __ANDROID__
					//This makes sure the (Cut|Copy|Paste) menu gets removed 
					//The previous way of removing the focus with .ClearFocus() would make the focus fall back to another textbox and reopen the keyboard on top of the modal.	
					if (FocusManager.GetFocusedElement() is TextBox textBox)
					{
						textBox.IsEnabled = false;
						textBox.IsEnabled = true;
					}
#endif
				}

				if (IsOtherPopupsClosed && isOpen)
				{
					foreach (var popup in VisualTreeHelper.GetOpenPopups(Windows.UI.Xaml.Window.Current))
					{
						popup.IsOpen = false;
					}
				}
			}
			catch (Exception ex)
			{
				this.Log().ErrorIfEnabled(() => $"Error executing strategies for MembershipCardControl: {ex.ToString()}");
			}
		}

		private enum DeviceOrientation
		{
			Portrait,
			Landscape
		}
	}
}
#endif
