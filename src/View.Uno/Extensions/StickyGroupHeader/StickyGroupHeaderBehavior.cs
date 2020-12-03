#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#if __ANDROID__ || __IOS__
using _FrameworkElement = Windows.UI.Xaml.FrameworkElement;
#else
using _FrameworkElement = Windows.UI.Xaml.FrameworkElement;
#endif

namespace Chinook.View.Extensions
{
	public static class StickyGroupHeaderBehavior
	{
		private static readonly List<WeakReference<_FrameworkElement>> _subscribedElements = new List<WeakReference<_FrameworkElement>>();

		private static readonly ConditionalWeakTable<ListViewBase, List<int>> _groupCounts = new ConditionalWeakTable<ListViewBase, List<int>>();

		public static object GetCurrentlyStickingGroup(DependencyObject obj)
		{
			return (object)obj.GetValue(CurrentlyStickingGroupProperty);
		}

		public static void SetCurrentlyStickingGroup(DependencyObject obj, object value)
		{
			obj.SetValue(CurrentlyStickingGroupProperty, value);
		}

		public static readonly DependencyProperty CurrentlyStickingGroupProperty =
			DependencyProperty.RegisterAttached("CurrentlyStickingGroup", typeof(object), typeof(StickyGroupHeaderBehavior), new PropertyMetadata(null));

		public static bool GetIsSticking(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsStickingProperty);
		}

		public static void SetIsSticking(DependencyObject obj, bool value)
		{
			obj.SetValue(IsStickingProperty, value);
		}

		public static readonly DependencyProperty IsStickingProperty =
			DependencyProperty.RegisterAttached("IsSticking", typeof(bool), typeof(StickyGroupHeaderBehavior), new PropertyMetadata(false));

		public static bool GetIsEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsEnabledProperty);
		}

		public static void SetIsEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsEnabledProperty, value);
		}

		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(StickyGroupHeaderBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				if (d is ListViewBase list)
				{
					SetUpListView(list);
				}
				else if (d is _FrameworkElement element)
				{
					SetUpElement(element);
				}
			}
		}

		private static void SetUpListView(ListViewBase listViewBase)
		{

			void onListViewBaseLoaded(object sender, RoutedEventArgs e)
			{
				listViewBase.Loaded -= onListViewBaseLoaded;

				var sv = listViewBase.FindFirstChild<ScrollViewer>();

				// Count items in each group to be able to calculate flat item index
				UpdateGroupCounts(listViewBase);
				listViewBase.RegisterPropertyChangedCallback(ItemsControl.ItemsSourceProperty, OnItemsSourceChanged);

				var wr = new WeakReference<ListViewBase>(listViewBase);

				sv.ViewChanged += (_, __) =>
				{
					var list = wr.GetTarget();
					UpdateCurrentlyStickingGroup(list);
				};

				UpdateCurrentlyStickingGroup(listViewBase);
			}
			listViewBase.Loaded += onListViewBaseLoaded;
		}

		private static void UpdateCurrentlyStickingGroup(ListViewBase list)
		{
			var stickingGroup = DetermineCurrentlyStickingGroup(list);
			SetCurrentlyStickingGroup(list, stickingGroup);
			foreach (var element in GetSubscribedDescendants(list))
			{
				SetIsSticking(element, (element as _FrameworkElement)?.DataContext?.Equals(stickingGroup) ?? false);
			}
		}

		/// <summary>
		/// Group is considered sticking if the uppermost (or leftmost) visible item belongs to it.
		/// </summary>
		private static object DetermineCurrentlyStickingGroup(ListViewBase listViewBase)
		{
			var panel = listViewBase.ItemsPanelRoot;
			var firstVisibleIndex = GetFirstVisibleIndex(panel);
			if (!_groupCounts.TryGetValue(listViewBase, out List<int> groupCounts))
			{
				throw new InvalidOperationException("");
			}

			var firstVisibleGroup = -1;
			var runningTotal = 0;
			for (int i = 0; i < groupCounts.Count; i++)
			{
				runningTotal += groupCounts[i];
				if (firstVisibleIndex < runningTotal)
				{
					firstVisibleGroup = i;
					break;
				}
			}
			if (firstVisibleGroup == -1)
			{
				return null;
			}
			return GetGroups(listViewBase)?.ElementAt(firstVisibleGroup);
		}

		private static void OnItemsSourceChanged(DependencyObject sender, DependencyProperty dp)
		{
			UpdateGroupCounts(sender as ListViewBase);
			UpdateCurrentlyStickingGroup(sender as ListViewBase);
		}

		private static void UpdateGroupCounts(ListViewBase listViewBase)
		{
			var counts = new List<int>();

			var groups = GetGroups(listViewBase);
			if (groups != null)
			{
				foreach (var group in groups)
				{
					counts.Add(group.Count());
				}
			}

			_groupCounts.Remove(listViewBase);

			_groupCounts.Add(listViewBase, counts);
		}

#if __ANDROID__ || __IOS__
		private static IEnumerable<IEnumerable> GetGroups(ListViewBase listViewBase)
		{
			return (listViewBase.ItemsSource as ICollectionView)?.CollectionGroups as IEnumerable<IEnumerable>;
		}
#else
		private static IEnumerable<IEnumerable> GetGroups(ListViewBase listViewBase)
		{
			return (listViewBase.ItemsSource as ICollectionView)?.CollectionGroups.Cast<ICollectionViewGroup>().Select(g => g.Group as IEnumerable);
		}
#endif

		private static void SetUpElement(_FrameworkElement element)
		{
			_subscribedElements.Add(new WeakReference<_FrameworkElement>(element));

			updateStickingGroup();
			element.Loaded += onLoaded;

			void onLoaded(object sender, RoutedEventArgs e)
			{
				element.Loaded -= onLoaded;
				// Element's DataContext is null when Loaded is called on iOS; dispatch update as a workaround
				element.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, updateStickingGroup);
			}

			void updateStickingGroup()
			{
				var list = element.FindFirstParent<ListViewBase>();
				if (list != null)
				{
					var stickingGroup = GetCurrentlyStickingGroup(list);
					SetIsSticking(element, (element as _FrameworkElement)?.DataContext?.Equals(stickingGroup) ?? false);
				}
			}
		}

		private static IEnumerable<_FrameworkElement> GetSubscribedDescendants(ListViewBase parentList) => GetAndCleanSubscribedElements()
			.Where(fe => fe.FindFirstParent<ListViewBase>() == parentList);

		private static readonly List<WeakReference<_FrameworkElement>> _toRemove = new List<WeakReference<_FrameworkElement>>();

		/// <summary>
		/// Enumerate listener elements and remove references to any that have been collected. 
		/// </summary>
		private static IEnumerable<_FrameworkElement> GetAndCleanSubscribedElements()
		{
			try
			{
				foreach (var wr in _subscribedElements)
				{
					var element = wr.GetTarget();
					if (element == null)
					{
						_toRemove.Add(wr);
					}
					else
					{
						yield return element;
					}

				}

				//Clean collected references
				foreach (var wr in _toRemove)
				{
					_subscribedElements.Remove(wr);
				}

			}
			finally
			{
				_toRemove.Clear();
			}
		}

		private static int GetFirstVisibleIndex(Panel virtualizingPanel)
		{
			return (virtualizingPanel as ItemsStackPanel)?.FirstVisibleIndex ??
				(virtualizingPanel as ItemsWrapGrid)?.FirstVisibleIndex ??
				throw new ArgumentException($"{nameof(StickyGroupHeaderBehavior)} only works in conjunction with {nameof(ItemsStackPanel)} or {nameof(ItemsWrapGrid)}");
		}
	}
}
#endif
