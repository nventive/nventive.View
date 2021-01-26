#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Uno.Extensions;
using Uno.Logging;
#if __IOS__
using UIKit;
#endif

namespace Nventive.View.Extensions
{
	/// <summary>
	/// Allows for setting the UIScrollViewIndicatorStyle of a ScrollViewer on iOS. Does not apply to other platforms
	/// </summary>
	public static partial class ScrollViewerExtensions
    {
		public enum IndicatorStyle: long
		{
			/// <summary>
			/// The default style of scroll indicator, which is black with a white border. This style is good against any content background.
			/// </summary>
			Default
#if __IOS__
				= UIScrollViewIndicatorStyle.Default
#endif
				,
			/// <summary>
			/// A style of indicator which is black and smaller than the default style. This style is good against a white content background.
			/// </summary>
			Black
#if __IOS__
				= UIScrollViewIndicatorStyle.Black
#endif
				,
			/// <summary>
			/// A style of indicator is white and smaller than the default style. This style is good against a black content background.
			/// </summary>
			White
#if __IOS__
				= UIScrollViewIndicatorStyle.White
#endif
		}

		public static void SetScrollIndicatorStyle(this Control element, IndicatorStyle scrollIndicatorStyle)
		{
			element.SetValue(ScrollIndicatorStyleProperty, scrollIndicatorStyle);
		}

		public static IndicatorStyle GetScrollIndicatorStyle(this Control element)
		{
			return (IndicatorStyle)element.GetValue(ScrollIndicatorStyleProperty);
		}

		public static DependencyProperty ScrollIndicatorStyleProperty { get; } =
			DependencyProperty.RegisterAttached(
				"ScrollIndicatorStyle",
				typeof(IndicatorStyle),
				typeof(ScrollViewerExtensions),
				new PropertyMetadata(IndicatorStyle.Default, OnScrollIndicatorStyleChanged)
			);

		private static void OnScrollIndicatorStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// ScrollIndicatorStyle is only a thing on iOS. The behavior is usable on other platforms for Xaml compatibility.
#if __IOS__
			if (d is Control control)
			{
				// Check if we can already fetch the UIScrollView from the child tree
				var sv = (control as UIView)?.FindFirstChild<UIScrollView>();

				if (sv == null)
				{
					// If the control wasn't already loaded, we want to listen to the Loaded event to access the UIScrollView
					control.Loaded += OnLoaded;

					void OnLoaded(object sender, RoutedEventArgs args) {
						control.Loaded -= OnLoaded; // Unregister event to avoid memory leaks

						sv = (control as UIView)?.FindFirstChild<UIScrollView>();
						if (sv == null)
						{
							control.Log().ErrorIfEnabled(() => "ScrollViewerExtensions.ScrollIndicatorStyle: Could not find a UIScrollView in the control's visual tree, aborting.");
							return;
						}

						// Set the indicator style
						sv.IndicatorStyle = (UIScrollViewIndicatorStyle)GetScrollIndicatorStyle(control);
					}
				}
				else
				{
					sv.IndicatorStyle = (UIScrollViewIndicatorStyle)GetScrollIndicatorStyle(control);
				}
			}
#endif
		}
	}
}
#endif
