#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Disposables;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.UI;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Nventive.View.Controls
{
	[TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
	public partial class PeekingFlipView : ListView
	{
		private const string PartScrollViewer = "ScrollViewer";

		private ScrollViewer _scrollViewer;
		private readonly SerialDisposable _subscriptions = new SerialDisposable();

		private bool _loaded;
		private bool _scrollingToIndex;
		private bool _isNotInitialLoad = false;
		private bool _manuallyScrolled = false; // Are we currently fixing the container, ie: scrolling manually
		private bool _checkSelection = true; // Does the selected item need to have position checked (for centering)
		private int _indexToCenter = 0; // Which index needs to be centered

		public PeekingFlipView()
		{
			_loaded = false;
			_scrollingToIndex = false;

			DefaultStyleKey = typeof(PeekingFlipView);
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;

			SelectionChanged += OnSelectionChanged;

			EnsureSubscribedToItemsSourceChanged();
		}

		protected void OnLoaded(object o, RoutedEventArgs args)
		{
			_loaded = true;

			_scrollViewer = (this.GetTemplateChild(PartScrollViewer) as ScrollViewer).Validation().NotNull(PartScrollViewer);
			_scrollViewer.ViewChanged += OnViewChanged;

			EnsureSubscribedToItemsSourceChanged();

			ScrollIntoViewSelected();
		}

		protected void OnUnloaded(object sender, RoutedEventArgs args)
		{
			_loaded = false;

			_scrollViewer.ViewChanged -= OnViewChanged;
			_subscriptions.Disposable = null;
		}

		private void EnsureSubscribedToItemsSourceChanged()
		{
			_subscriptions.Disposable = this.ObservePropertyChanged<object>(ItemsSourceProperty)
				.Subscribe(_ => SetInitialSelection());
		}

		private void OnSelectionChanged(object s, SelectionChangedEventArgs e)
		{
			if (_loaded)
			{
				ScrollIntoViewSelected();
				PreventTapOnNonSelectedItems(e.RemovedItems?.FirstOrDefault(), e.AddedItems?.FirstOrDefault());

				// Selection may be reset if there is a collection change that removes the selected item.
				if (SelectedIndex == -1)
				{
					_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
					{
						if (SelectedIndex == -1)
						{
							SetSelectedIndex();
						}
					});
				}

				// Don't check on first load
				if (_isNotInitialLoad)
				{
					// Check for selectedItem centering
					CheckAsync();
				}
				else
				{
					_isNotInitialLoad = true;
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("void async", "NV0016", Justification = "Fixes bug")]
		private async void CheckAsync()
		{
			// Wait until selectedIndex of the PeekingFlipView is in view to check if it needs centering
			while (!IsInView())
			{
#if __IOS__
				await Task.Delay(25, CancellationToken.None);
#else
				// Since Android code and ui is on the same thread if the delay is shorter we may causes flickering
				await Task.Delay(100, CancellationToken.None);
#endif
			}

#if __ANDROID__
			// Without this delay concurrency errors occur in the ui, when items that are next to eachother are selected
			await Task.Delay(25, CancellationToken.None);
#endif

			if (_checkSelection)
			{
				FixCenterSnapPoint();
			}
		}

		// This method places SelectedItem into the center of the container
		private void FixCenterSnapPoint()
		{
			var panel = this.ItemsPanelRoot as ItemsStackPanel;
			var items = ItemsSource as IEnumerable;
			int selectedItemIndex = 0;

			// If we re-enter this method before the selection is placed into the center, reset the indexToSelect to complete selection
			if (_checkSelection)
			{
				selectedItemIndex = items?.IndexOf(SelectedItem) ?? -1;
				_indexToCenter = selectedItemIndex;
				_checkSelection = false;
			}
			else
			{
				selectedItemIndex = _indexToCenter;
			}

			// Find selected index container if in view
			var container = (SelectorItem)ContainerFromIndex(selectedItemIndex);
			var offset = GetRelativeOffset(container);

			// Flipviews measures
			double viewCenter = this.ActualWidth / 2;
			var leftBound = offset.X;
			var rightBound = leftBound + container.ActualWidth;

			// If selected item is touching the left/right side of the flipview scroll it into the middle 
			if (leftBound < 0)
			{
#if __IOS__
				// IOS Scrolling by the center offset doesn't perfectly place the selected item in the center 
				_scrollViewer.ChangeView(_scrollViewer.HorizontalOffset - (7 * viewCenter / 9), null, null, false);
#else
				_scrollViewer.ChangeView(_scrollViewer.HorizontalOffset - viewCenter, null, null, false);
#endif
				_manuallyScrolled = true;
			}
			else if (rightBound > this.ActualWidth)
			{
#if __IOS__
				// IOS Scrolling by the center offset doesn't perfectly place the selected item in the center 
				_scrollViewer.ChangeView(_scrollViewer.HorizontalOffset + (4 * viewCenter / 5), null, null, false);
#else
				_scrollViewer.ChangeView(_scrollViewer.HorizontalOffset + viewCenter, null, null, false);
#endif
				_manuallyScrolled = true;
			}

			_checkSelection = true;
		}

		// Is SelectedIndex in the PeekingFlipView
		private bool IsInView()
		{
			var panel = this.ItemsPanelRoot as ItemsStackPanel;

#pragma warning disable Uno0001 // Uno type or member is not implemented
			if (panel.FirstVisibleIndex <= SelectedIndex && SelectedIndex <= panel.LastVisibleIndex)
			{
				return true;
			}

			return false;
		}

		private void ScrollIntoViewSelected()
		{
			_scrollingToIndex = true;
			ScrollIntoView(SelectedItem);
		}

		private void PreventTapOnNonSelectedItems(object unselectedItem, object selectedItem)
		{
			// Prevent non selected items from being selected via tapping on the item
			if (unselectedItem != null)
			{
				var unselectedItemContainer = (SelectorItem)ContainerFromItem(unselectedItem);
				if (unselectedItemContainer != null)
				{
					unselectedItemContainer.IsHitTestVisible = false;
				}
			}

			if (selectedItem != null)
			{
				var selectedItemContainer = (SelectorItem)ContainerFromItem(selectedItem);
				if (selectedItemContainer != null)
				{
					selectedItemContainer.IsHitTestVisible = true;
				}
			}
		}

		private void SetInitialSelection()
		{
			var items = ItemsSource as IEnumerable;

			if (items != null && items.Any())
			{
				SelectedIndex = 0;
			}
		}

		// Select index based on whichever item is centered in the control
		private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs args)
		{
			// if we were manually setting SelectedIndex and now it is in view, reset manuallyScrolling
			if (_manuallyScrolled && IsInView())
			{
				_manuallyScrolled = false;
			}
			else
			{
				//We only want to change selected index when the view has stopped scrolling
				if (args.IsIntermediate || _scrollingToIndex)
				{
					_scrollingToIndex = false;
					return;
				}

				SetSelectedIndex();
			}
		}

		private void SetSelectedIndex()
		{
			var panel = this.ItemsPanelRoot as ItemsStackPanel;

			double viewCenter = this.ActualWidth / 2;
			int indexToSelect = -1;

#pragma warning disable Uno0001 // Uno type or member is not implemented
			if (panel.FirstVisibleIndex == -1)
			{
				return;
			}

			for (int i = panel.FirstVisibleIndex; i <= panel.LastVisibleIndex; i++)
			{
				var container = (SelectorItem)ContainerFromIndex(i);

				var offset = GetRelativeOffset(container);
				var leftBound = offset.X;
				var rightBound = leftBound + container.ActualWidth;

				if (leftBound < viewCenter && rightBound > viewCenter)
				{
					indexToSelect = i;
					break;
				}
			}
#pragma warning restore Uno0001 // Uno type or member is not implemented

			if (indexToSelect > -1)
			{
				this.SelectedIndex = indexToSelect;
			}
		}

		private Point GetRelativeOffset(UIElement item)
		{
#if __ANDROID__
			var containerPosition = GetLocationOnScreen(this);
			var itemPosition = GetLocationOnScreen(item);
			var offset = ViewHelper.PhysicalToLogicalPixels(itemPosition - containerPosition);

			Point GetLocationOnScreen(UIElement e)
			{
				var buffer = new int[2];
				e.GetLocationOnScreen(buffer);

				return new Point(buffer[0], buffer[1]);
			}
#elif __IOS__ || __MACOS__
			var unit = new CoreGraphics.CGRect(0, 0, 1, 1);
			var transform = this.ConvertRectFromView(unit, item);
			var offset = new Point(transform.X, transform.Y);
#else
			var transform = (item as UIElement).TransformToVisual(this);
			var offset = transform.TransformPoint(new Point());
#endif
			return offset;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			if (element is SelectorItem selectorItem)
			{
				// Prevent non selected items from being selected via tapping on the item when the list reloads
				var items = ItemsSource as IEnumerable;

				var itemIndex = items?.IndexOf(item) ?? -1;
				var isInitialSelection = SelectedItem == null && itemIndex == 0;

				selectorItem.IsHitTestVisible = isInitialSelection || (SelectedItem?.Equals(item) ?? false);

				//Not being set automatically, as it is with ListViewItem.
				//TODO #103030: Are there any more properties that need to be assigned this way?
				selectorItem.ContentTemplate = this.ItemTemplate;
			}
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new PeekingFlipViewItem();
		}
	}
}
#endif
