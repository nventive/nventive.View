#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.Logging;
using Uno.Logging;

namespace Nventive.View.Controls
{
	/// <summary>
	/// A control which wraps a scrolling container (eg, a ScrollViewer or a ListView) and exposes the scrolled offset relative to a defined header height.
	/// </summary>
	public partial class AnimatedHeaderControl : ContentControl
	{
		private FrameworkElement _mastheadElement;
		private ScrollViewer _scrollViewer;

		/// <summary>
		/// Set this value to tell the control where the masthead ends. RelativeOffset will be 1 when the AbsoluteOffset reaches this value.
		/// </summary>
		public double MastheadEnd
		{
			get { return (double)GetValue(MastheadEndProperty); }
			set { SetValue(MastheadEndProperty, value); }
		}

		public static readonly DependencyProperty MastheadEndProperty =
			DependencyProperty.Register("MastheadEnd", typeof(double), typeof(AnimatedHeaderControl), new PropertyMetadata(-1d));


		/// <summary>
		/// The margin above the masthead. This can be used to adjust <see cref="RelativeOffset"/> for an overlaid element, eg the command bar.
		/// </summary>
		public double MastheadTopMargin
		{
			get { return (double)GetValue(MastheadTopMarginProperty); }
			set { SetValue(MastheadTopMarginProperty, value); }
		}

		public static readonly DependencyProperty MastheadTopMarginProperty =
			DependencyProperty.Register("MastheadTopMargin", typeof(double), typeof(AnimatedHeaderControl), new PropertyMetadata(0d));

		/// <summary>
		/// The scrolled offset in absolute pixel values.
		/// </summary>
		public double AbsoluteOffset
		{
			get { return (double)GetValue(AbsoluteOffsetProperty); }
			set { SetValue(AbsoluteOffsetProperty, value); }
		}

		public static readonly DependencyProperty AbsoluteOffsetProperty =
			DependencyProperty.Register("AbsoluteOffset", typeof(double), typeof(AnimatedHeaderControl),
				new PropertyMetadata(0d, (o, e) => ((AnimatedHeaderControl)o).OnAbsoluteOffsetChanged((double)e.NewValue)));

		private void OnAbsoluteOffsetChanged(double newValue)
		{
			RelativeOffset = CalculateRelativeOffset(newValue);
		}

		/// <summary>
		/// The fractional offset relative to the end of the header. This will always be a value between 0 (start of the scrolled content) and 1 (header fully scrolled out of view).
		/// </summary>
		public double RelativeOffset
		{
			get { return (double)GetValue(RelativeOffsetProperty); }
			set { SetValue(RelativeOffsetProperty, value); }
		}

		public static readonly DependencyProperty RelativeOffsetProperty =
			DependencyProperty.Register("RelativeOffset", typeof(double), typeof(AnimatedHeaderControl), new PropertyMetadata(0d));

		public static bool GetIsMasthead(FrameworkElement frameworkElement)
		{
			return (bool)frameworkElement.GetValue(IsMastheadProperty);
		}

		/// <summary>
		/// Setting 'AnimatedHeaderControl.IsMasthead="True"' on an element will cause the bottom of that element to be used when calculating <see cref="RelativeOffset"/>.
		/// </summary>
		public static void SetIsMasthead(FrameworkElement frameworkElement, bool value)
		{
			frameworkElement.SetValue(IsMastheadProperty, value);
		}

		// Using a DependencyProperty as the backing store for IsMasthead.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsMastheadProperty =
			DependencyProperty.RegisterAttached("IsMasthead", typeof(bool), typeof(AnimatedHeaderControl), new PropertyMetadata(false, OnIsMastheadChanged));

		private static void OnIsMastheadChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if ((bool)args.NewValue)
			{
				// Set marked element as masthead, either now or when loaded
				var element = dependencyObject as FrameworkElement;

				bool trySetAsMasthead()
				{
					var owner = element.FindFirstParent<AnimatedHeaderControl>();
					if (owner != null)
					{
						owner._mastheadElement = element;
						owner.MastheadEnd = -1d;
						return true;
					}
					else
					{
						return false;
					}
				}

				var set = trySetAsMasthead();

				if (!set)
				{
					void onLoaded(object sender, RoutedEventArgs e)
					{
						element.Loaded -= onLoaded;
						trySetAsMasthead();
					}
					element.Loaded += onLoaded;
				}
			}
		}

		public AnimatedHeaderControl()
		{
			DefaultStyleKey = typeof(AnimatedHeaderControl);

			Loaded += (o, e) => SubscribeToViewChanged();
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			SubscribeToViewChanged();
		}

		private void SubscribeToViewChanged()
		{
			if (_scrollViewer == null)
			{
				if (Content is Control control)
				{
#if __ANDROID__ || __IOS__
					// TODO #193121 - Make sure this code is still needed
					// It's possible the Template property is not set yet.
					// This is because the style setters have not been called yet.
					// We call the style explicitly so that the Template is set.
					// This way, we can assume we will be able to get the child ScrollViewer.
					//if (control.Template == null)
					//{
					//	var style = control.Style ?? Style.DefaultStyleForType(control.GetType());
					//	style.ApplyTo(control);
					//}
#endif

					control.ApplyTemplate();
				}

				_scrollViewer = this.FindFirstChild<ScrollViewer>() ?? (Content as DependencyObject)?.FindFirstChild<ScrollViewer>();

				// If no scrollviewer found, look for a ListViewBase, materialize it, then atttempt to get a scrollviewer from it
				if (_scrollViewer == null)
				{
					var lvb = this.FindFirstChild<ListViewBase>() ?? (Content as DependencyObject)?.FindFirstChild<ListViewBase>();

					if (lvb != null)
					{
#if __ANDROID__ || __IOS__
						// TODO #193121 - Make sure this code is still needed
						//var style = lvb.Style ?? Style.DefaultStyleForType(lvb.GetType());
						//style.ApplyTo(lvb);
#endif
						lvb?.ApplyTemplate();
						_scrollViewer = lvb?.FindFirstChild<ScrollViewer>();
					}
				}

				if (_scrollViewer != null)
				{
					void onViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
					{
						AbsoluteOffset = _scrollViewer.VerticalOffset;
					}

					_scrollViewer.ViewChanged += onViewChanged;
#if __ANDROID__ || __IOS__
					RegisterLoadActions(loaded: () =>
						{
							_scrollViewer.ViewChanged -= onViewChanged;
							_scrollViewer.ViewChanged += onViewChanged;
							onViewChanged(_scrollViewer, null);
						},
						unloaded: () =>
						{
							_scrollViewer.ViewChanged -= onViewChanged;
						}
					);
#endif
				}
			}
		}

		private double GetMastheadEnd()
		{
			if (MastheadEnd != -1d)
			{
				return MastheadEnd;
			}

			if (_mastheadElement != null && _scrollViewer != null)
			{
				// Find position of masthead relative to scrolling content
#if WINDOWS_UWP
				// On UWP we can count on finding a ScrollContentPresenter. 
				var scp = _scrollViewer.FindFirstChild<ScrollContentPresenter>();
				var content = scp?.Content as FrameworkElement;
				var transform = _mastheadElement.TransformToVisual(content);
				var mastheadElementStart = transform.TransformPoint(new Point(0, 0)).Y + content.Margin.Top;
				MastheadEnd = mastheadElementStart + _mastheadElement.ActualHeight;
#else
				// In Uno we can count on the VerticalOffset being 'in sync' with what TransformToVisual reports. (On UWP this isn't the 
				// case, maybe because of background composition.)
				var transform = _mastheadElement.TransformToVisual(this);
				var mastheadElementStart = transform.TransformPoint(new Point(0, 0)).Y + _scrollViewer.VerticalOffset;
				MastheadEnd = mastheadElementStart + _mastheadElement.ActualHeight;
#endif
				return MastheadEnd;
			}

			return -1d;
		}

		private double CalculateRelativeOffset(double absoluteOffset)
		{
			var mastheadEnd = GetMastheadEnd();
			if (mastheadEnd <= 0)
			{
				if (this.Log().IsEnabled(LogLevel.Warning))
				{
					this.Log().Warn($"{nameof(MastheadEnd)} is unset, ${nameof(RelativeOffset)} will always be 0");
				}
				return 0;
			}

			double offset = absoluteOffset / (mastheadEnd - MastheadTopMargin);

			// Clamp value
			return Math.Max(0, Math.Min(offset, 1));
		}
	}
}
#endif
