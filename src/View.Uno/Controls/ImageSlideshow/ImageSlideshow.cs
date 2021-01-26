#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.Collections.Specialized;
using Uno.Disposables;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace Nventive.View.Controls
{
	[TemplatePart(Name = "PART_FlipView", Type = typeof(Selector))]
	[TemplatePart(Name = "PART_ItemsControl", Type = typeof(ItemsControl))]
	public partial class ImageSlideshow : Control
	{
		private const string PartFlipView = "PART_FlipView";
		private const string PartItemsControl = "PART_ItemsControl";

		private readonly SerialDisposable _moveToNextDisposable = new SerialDisposable();
		private readonly SerialDisposable _updateOnCollectionChangedDisposable = new SerialDisposable();
		private bool _isGoingBackToStart;

		private bool _isSelectedIndexSynchronized;

		private bool _isViewReady;
		private bool _isLoaded;

		private Selector _flipView;
		private ItemsControl _itemsControl;

		private IEnumerable ItemsSourceInner => ItemsSource as IEnumerable;

		private int ItemsCount => (ItemsSourceInner?.Count()).GetValueOrDefault();

		public ImageSlideshow()
		{
			DefaultStyleKey = typeof(ImageSlideshow);
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_itemsControl = (this.GetTemplateChild(PartItemsControl) as ItemsControl).Validation().NotNull(PartItemsControl);
			_flipView = (this.GetTemplateChild(PartFlipView) as Selector).Validation().NotNull(PartFlipView);

			_flipView.SelectionChanged -= OnFlipViewItemChanged;
			_flipView.SelectionChanged += OnFlipViewItemChanged;

			SetItemsSource(true);
			_isViewReady = true;

			Update();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;

			_flipView.SelectionChanged -= OnFlipViewItemChanged;
			_flipView.SelectionChanged += OnFlipViewItemChanged;

			Update();

		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			_moveToNextDisposable.Disposable = null;

			if (_itemsControl != null)
			{
				_itemsControl.ItemsSource = null;
				_itemsControl.ItemTemplateSelector = null;
			}

			if (_flipView != null)
			{
				_flipView.SelectionChanged -= OnFlipViewItemChanged;
			}
		}

		private void OnItemsSourceChanged()
		{
			SetItemsSource(ShouldPreserveSelectedIndex);

			if (ItemsSource is INotifyCollectionChanged itemSource)
			{
				_updateOnCollectionChangedDisposable.Disposable = Disposable.Create(() => itemSource.CollectionChanged -= Update);
				itemSource.CollectionChanged += Update;
			}
			else
			{
				_updateOnCollectionChangedDisposable.Disposable = null;
			}

			Update();
		}

		private void SetItemsSource(bool preserveSelectedIndex)
		{
			if (_flipView != null)
			{
				if (preserveSelectedIndex)
				{
					var flipView = _flipView as FlipView;

					// We disable animations when setting the selected index and then restore it.
					// This is a workaround to prevent a bug on the iOS FlipView (#101514)
					var useTouchAnimationsForAllNavigations = flipView?.UseTouchAnimationsForAllNavigation;

					try
					{
						if (flipView != null && useTouchAnimationsForAllNavigations != null)
						{
#pragma warning disable Uno0001 // Uno type or member is not implemented for WASM
							flipView.UseTouchAnimationsForAllNavigation = false;
#pragma warning restore Uno0001 // Uno type or member is not implemented for WASM
						}

						var flipViewIndex = Math.Max(_flipView.SelectedIndex, 0);
						var selectedIndex = Math.Min(flipViewIndex, ItemsCount - 1);

						_flipView.ItemsSource = ItemsSource;
						_flipView.SelectedIndex = selectedIndex;
					}
					finally
					{
						if (flipView != null && useTouchAnimationsForAllNavigations != null)
						{
#pragma warning disable Uno0001 // Uno type or member is not implemented for WASM
							flipView.UseTouchAnimationsForAllNavigation = useTouchAnimationsForAllNavigations.Value;
#pragma warning restore Uno0001 // Uno type or member is not implemented for WASM
						}
					}
				}
				else
				{
					SelectedIndex = 0;
					_flipView.ItemsSource = ItemsSource;
				}
			}
		}

		private void Update(object sender, NotifyCollectionChangedEventArgs e) => Update();

		private void Update()
		{
			if (_isLoaded && _isViewReady)
			{
				_itemsControl.ItemTemplateSelector = new IndexIndicatorTemplateSelector
				{
					SelectedTemplate = SelectedIndexIndicatorTemplate,
					DefaultTemplate = IndexIndicatorTemplate
				};

				SynchronizeSelection();

				UpdateIndicators();

				OnAutoRotateChanged();
			}
		}

		private void UpdateIndicators()
		{
			var hasMultipleItems = ItemsCount > 1;

			if (hasMultipleItems)
			{
				var itemsSource = new List<IndexIndicator>();
				var index = 0;

				foreach (var item in ItemsSourceInner)
				{
					itemsSource.Add(new IndexIndicator
					{
						DataContext = item,
						IsSelected = index++ == SelectedIndex
					});
				}

				_itemsControl.ItemsSource = itemsSource.ToArray();
			}

			// If we no longer have multiple items, simply remove the indicators.
			else if (_itemsControl.ItemsSource != null)
			{
				_itemsControl.ItemsSource = null;
			}
		}

		private void MoveToNext()
		{
			var itemsCount = ItemsCount;

			if (itemsCount == 0)
			{
				return;
			}

			if (SelectedIndex >= itemsCount)
			{
				_isGoingBackToStart = true;
			}
			else if (SelectedIndex == 0)
			{
				_isGoingBackToStart = false;
			}

			if (_isGoingBackToStart && !AutoRotateRewindEnabled)
			{
				return;
			}

			int newIndex = SelectedIndex;

			TimeSpan moveToNextDelay = TimeSpan.Zero;

			if (_isGoingBackToStart)
			{
				if (newIndex >= itemsCount)
				{
					newIndex = itemsCount - 2;
				}
				else
				{
					newIndex--;
				}

				moveToNextDelay = TimeSpan.FromMilliseconds(RewindTime);
			}
			else
			{
				newIndex++;

				moveToNextDelay = TimeSpan.FromMilliseconds(DisplayTime);
			}

			_ = Dispatcher.RunTaskAsync(CoreDispatcherPriority.Normal, async () =>
			{
				await Task.Delay(moveToNextDelay);

				SelectedIndex = newIndex;
			});
		}

		private void OnAutoRotateChanged()
		{
			if (_isLoaded && _isViewReady)
			{
				if (AutoRotate)
				{
					MoveToNext();
				}
				else
				{
					_moveToNextDisposable.Disposable = null;
				}
			}
		}

		private void OnSelectedIndexChanged()
		{
			if (_isLoaded && _isViewReady)
			{
				CheckAndSetSelectedIndexOnFlipView();

				UpdateIndicators();

				if (AutoRotate)
				{
					MoveToNext();
				}
			}
		}

		//We call this function where we want to do the following assignment:
		//	_flipView.SelectedIndex = this.SelectedIndex;
		// ...However, if we do this when this.SelectedIndex >= the count of items currently held in the flipview,
		// we get an IndexOutOfRangeException. So, wrap the assignment in a check,
		// and clamp this.SelectedIndex if appropriate.
		private void CheckAndSetSelectedIndexOnFlipView()
		{
			var itemsEnumerable = (_flipView.ItemsSource as IEnumerable);
			var itemsCount = ItemsCount;

			if (itemsCount > 0)
			{
				if (SelectedIndex < itemsCount)
				{
					_flipView.SelectedIndex = SelectedIndex;
				}
				else
				{
					Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
					{
						SelectedIndex = itemsCount - 1;
					});
				}
			}
		}

		private void OnFlipViewItemChanged(object sender, SelectionChangedEventArgs e) => SynchronizeSelection();

		//This function is how we synchronize the _flipView.SelectedIndex with this.SelectedIndex.
		//Whenever the flipView's ItemsSource property is set, the flipView sets its SelectedIndex
		// first to -1, and then to 0. We want to ignore these two values, but then listen for and
		// update this.SelectedIndex based on subsequent changes.
		private void SynchronizeSelection()
		{
			if (_flipView.SelectedIndex >= 0)
			{
				if (_isSelectedIndexSynchronized)
				{
					SelectedIndex = _flipView.SelectedIndex;
				}
				else
				{
					_isSelectedIndexSynchronized = true;
					Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
					{
						CheckAndSetSelectedIndexOnFlipView();
					});
				}
			}
			else
			{
				_isSelectedIndexSynchronized = false;
			}
		}

		private class IndexIndicatorTemplateSelector : DataTemplateSelector
		{
			public DataTemplate SelectedTemplate { get; set; }

			public DataTemplate DefaultTemplate { get; set; }

			protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
			{
				return SelectTemplateCore(item);
			}

			protected override DataTemplate SelectTemplateCore(object item)
			{
				var indexIndicator = item as IndexIndicator;

				if (indexIndicator != null)
				{
					return indexIndicator.IsSelected ? SelectedTemplate : DefaultTemplate;
				}

				return DefaultTemplate;
			}
		}

		private class IndexIndicator
		{
			public object DataContext { get; set; }

			public bool IsSelected { get; set; }
		}
	}
}
#endif
