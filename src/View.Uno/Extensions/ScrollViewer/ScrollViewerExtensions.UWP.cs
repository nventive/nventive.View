#if WINDOWS_UWP
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Nventive.View.Extensions
{
	static public partial class ScrollViewerExtensions
	{
		/// <summary>
		/// Scroll the desired element into the ScrollViewer's viewport.
		/// </summary>
		/// <param name="viewer">The ScrollViewer.</param>
		/// <param name="element">The element to scroll into view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="viewer" /> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element" /> is null.
		/// </exception>
		public static void ScrollIntoView(this ScrollViewer viewer, FrameworkElement element)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			// Note: no need to handle the IDisposable, because it's doing nothing in this case
			viewer.ScrollIntoView(element, 0.0, 0.0, TimeSpan.Zero);
		}

		/// <summary>
		/// Scroll the desired element into the ScrollViewer's viewport.
		/// </summary>
		/// <remarks>
		/// Disposing the result will terminate the animation (if any)
		/// </remarks>
		/// <param name="viewer">The ScrollViewer.</param>
		/// <param name="element">The element to scroll into view.</param>
		/// <param name="horizontalMargin">The margin to add on the left or right.
		/// </param>
		/// <param name="verticalMargin">The margin to add on the top or bottom.
		/// </param>
		/// <param name="duration">The duration of the animation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="viewer" /> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element" /> is null.
		/// </exception>
		public static IDisposable ScrollIntoView(this ScrollViewer viewer, FrameworkElement element, double horizontalMargin = 0.0, double verticalMargin = 0.0, TimeSpan duration = default(TimeSpan))
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			var itemRect = GetBoundsRelativeTo(element, viewer);
			if (!itemRect.HasValue)
			{
				return Disposable.Empty;
			}

			var newVerticalOffset = CalculateNewOffset(verticalMargin, viewer.ViewportHeight, itemRect.Value.Top, itemRect.Value.Bottom, viewer.VerticalOffset);
			var newHorizontalOffset = CalculateNewOffset(horizontalMargin, viewer.ViewportWidth, itemRect.Value.Left, itemRect.Value.Right, viewer.HorizontalOffset);

			if (duration == TimeSpan.Zero)
			{
				viewer.ChangeView(null, newVerticalOffset, null);
				viewer.ChangeView(newHorizontalOffset, null, null);
				return Disposable.Empty;
			}

			// Create Storyboard for animation
			var storyboard = new Storyboard();

			var verticalOffsetAnimation = new DoubleAnimation
			{
				To = newVerticalOffset,
				Duration = duration
			};
			Storyboard.SetTarget(verticalOffsetAnimation, viewer);

			var horizontalOffsetAnimation = new DoubleAnimation
			{
				To = newHorizontalOffset,
				Duration = duration
			};
			Storyboard.SetTarget(horizontalOffsetAnimation, viewer);

			//Storyboard.SetTargetProperty(horizontalOffsetAnimation, new PropertyPath(ScrollViewer.HorizontalOffsetProperty));
			//Storyboard.SetTargetProperty(verticalOffsetAnimation, new PropertyPath(ScrollViewer.VerticalOffsetProperty));

			storyboard.Children.Add(verticalOffsetAnimation);
			storyboard.Children.Add(horizontalOffsetAnimation);

			// Start animating
			storyboard.Begin();

			return Disposable.Create(storyboard.Stop);
		}

		private static double CalculateNewOffset(double margin, double size, double topPosition, double bottomPosition, double currentOffset)
		{
			var verticalDelta = 0.0;
			var hostBottom = size;
			var itemBottom = bottomPosition + margin;

			if (hostBottom < itemBottom)
			{
				verticalDelta = itemBottom - hostBottom;
				currentOffset += verticalDelta;
			}

			var itemTop = topPosition - margin;
			if (itemTop - verticalDelta < 0.0)
			{
				currentOffset -= verticalDelta - itemTop;
			}
			return currentOffset;
		}
		
		/// <summary>
		/// Observe when manipulations such as scrolling and zooming cause the view to change
		/// </summary>
		internal static IObservable<EventPattern<ScrollViewerViewChangingEventArgs>> ObserveViewChanging(
			this ScrollViewer scrollViewer,
			UiEventSubscriptionsOptions options = UiEventSubscriptionsOptions.Default)
		{
			return ObservableExtensions.FromEventPattern<EventHandler<ScrollViewerViewChangingEventArgs>, ScrollViewerViewChangingEventArgs>(
				h => scrollViewer.ViewChanging += h,
				h => scrollViewer.ViewChanging -= h,
				scrollViewer,
				options);
		}

		/// <summary>
		/// Observe when manipulations such as scrolling and zooming have caused the view to change.
		/// </summary>
		internal static IObservable<EventPattern<ScrollViewerViewChangedEventArgs>> ObserveViewChanged(
			this ScrollViewer scrollViewer,
			UiEventSubscriptionsOptions options = UiEventSubscriptionsOptions.Default)
		{
			return ObservableExtensions.FromEventPattern<EventHandler<ScrollViewerViewChangedEventArgs>, ScrollViewerViewChangedEventArgs>(
				h => scrollViewer.ViewChanged += h,
				h => scrollViewer.ViewChanged -= h,
				scrollViewer,
				options);
		}
		
		public static Rect? GetBoundsRelativeTo(FrameworkElement element, UIElement otherElement)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (otherElement == null)
			{
				throw new ArgumentNullException("otherElement");
			}
			try
			{
				GeneralTransform transform = element.TransformToVisual(otherElement);
				Point origin;
				Point bottom;
				if (transform != null && transform.TryTransform(default(Point), out origin) && transform.TryTransform(new Point(element.ActualWidth, element.ActualHeight), out bottom))
				{
					return new Rect(origin, bottom);
				}
			}
			catch (ArgumentException)
			{
			}
			return default(Rect?);
		}
	}
}
#endif
