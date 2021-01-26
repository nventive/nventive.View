#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
#if WINDOWS_UWP
using IFrameworkElement = Windows.UI.Xaml.FrameworkElement;
#elif __IOS__
using UIKit;
using Uno.Extensions;
#endif

namespace Nventive.View.Extensions
{
	public class ExpandIntoOverscrollBehavior
	{
		public static bool GetIsEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsEnabledProperty);
		}

		public static void SetIsEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsEnabledProperty, value);
		}

		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ExpandIntoOverscrollBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Overscroll is only a thing on iOS. The behavior is usable on other platforms for Xaml compatibility.
#if __IOS__
			if (d is FrameworkElement targetElement)
			{
				// We need to refer to these two variables in the nested methods below.
				EventHandler<ScrollViewerViewChangedEventArgs> handler = null;
				ScrollViewer sv = null;

				if ((bool)e.NewValue)
				{
					var transform = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
					targetElement.RenderTransform = transform;
					targetElement.RenderTransformOrigin = new Point(0.5, 1);

					targetElement.Loaded += onLoaded;
					targetElement.Unloaded += onUnloaded;

					onLoaded(targetElement, null);
				}
				else
				{
					targetElement.Loaded -= onLoaded;
					targetElement.Unloaded -= onUnloaded;

					onUnloaded(targetElement, null);
				}

				void onLoaded(object sender, RoutedEventArgs args)
				{
					var view = sender as UIView;

					sv = view.FindFirstParent<ScrollViewer>();
					if (sv == null)
					{
						return;
					}

					EnsureNoClip(view, sv);
					sv.ShouldReportNegativeOffsets = true;
					var wr = new WeakReference<FrameworkElement>(sender as FrameworkElement);

					handler = (o, e2) => OnViewChanged(o as ScrollViewer, wr);

					sv.ViewChanged += handler;
				}

				void onUnloaded(object sender, RoutedEventArgs args)
				{
					if (sv != null && handler != null)
					{
						sv.ViewChanged -= handler;
					}
				}
			}
#endif
		}
#if __IOS__

		private static void OnViewChanged(ScrollViewer scrollViewer, WeakReference<FrameworkElement> targetElementRef)
		{
			var targetElement = targetElementRef.GetTarget();
			if (targetElement == null)
			{
				return;
			}

			double scale;
			if (scrollViewer.VerticalOffset < 0)
			{
				var height = targetElement.ActualHeight;
				var expansion = height != 0 ?
					-scrollViewer.VerticalOffset / height :
					0;
				scale = 1 + expansion;
			}
			else
			{
				scale = 1;
			}

			var transform = targetElement.RenderTransform as ScaleTransform;
			transform.ScaleX = scale;
			transform.ScaleY = scale;
		}

		/// <summary>
		/// Disable clipping up to the level of the outer view. This ensures that the scaled-up element isn't cut off. 
		/// </summary>
		private static void EnsureNoClip(UIView innerview, UIView outerView)
		{
			UIView parent = innerview;
			while (true)
			{
				parent = parent.Superview;
				if (parent == outerView || parent == null)
				{
					break;
				}
				parent.ClipsToBounds = false;
			}
		}
#endif
	}
}
#endif
