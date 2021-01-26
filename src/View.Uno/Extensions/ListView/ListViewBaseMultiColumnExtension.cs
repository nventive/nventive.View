#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	/// <summary>
	/// Extension that allows to update the listview template's size when the size of the screen changes,
	/// keeping the NumberOfColumns and ItemSizeRatio defined.
	/// </summary>
	public class ListViewBaseMultiColumnExtension
	{
		#region NumberOfColumns
		/// <summary>
		/// Get the NumberOfColumns, which defines how many items appear in each row of list view
		/// </summary>
		public static int GetNumberOfColumns(DependencyObject obj)
		{
			return (int)obj.GetValue(NumberOfColumnsProperty);
		}

		/// <summary>
		/// Set the NumberOfColumns, which defines how many items appear in each row of list view
		/// </summary>
		public static void SetNumberOfColumns(DependencyObject obj, int value)
		{
			obj.SetValue(NumberOfColumnsProperty, value);
		}

		/// <summary>
		/// Exposes NumberOfColumns property let the developer define how many items appear in each row of list view
		/// </summary>
		public static readonly DependencyProperty NumberOfColumnsProperty =
			DependencyProperty.RegisterAttached("NumberOfColumns", typeof(int), typeof(ListViewBaseMultiColumnExtension), new PropertyMetadata(0, (d, e) => OnNumberOfColumnsChanged((ListView)d, (int)e.NewValue)));

		/// <summary>
		/// Handles value change of NumberOfColumns property
		/// </summary>
		private static void OnNumberOfColumnsChanged(ListView listView, int numberOfColumns)
		{
			SubscribeToSizeChanged(listView, numberOfColumns, GetItemSizeRatio(listView), GetMarginBetweenColumns(listView));
		}
		#endregion

		#region ItemSizeRatio
		/// <summary>
		/// Get the ItemSizeRatio, which defines the height/ width ratio of each list view template
		/// </summary>
		public static double GetItemSizeRatio(DependencyObject obj)
		{
			return (double)obj.GetValue(ItemSizeRatioProperty);
		}

		/// <summary>
		/// Set the ItemSizeRatio, which defines the height/ width ratio of each list view template
		/// </summary>
		public static void SetItemSizeRatio(DependencyObject obj, double value)
		{
			obj.SetValue(ItemSizeRatioProperty, value);
		}

		/// <summary>
		/// Exposes ItemSizeRatioProperty property let the developer define the height/ width ratio of each list view template
		/// </summary>
		public static readonly DependencyProperty ItemSizeRatioProperty =
			DependencyProperty.RegisterAttached("ItemSizeRatio", typeof(double), typeof(ListViewBaseMultiColumnExtension), new PropertyMetadata(0d, (d, e) => OnItemSizeRatioChanged((ListView)d, (double)e.NewValue)));

		/// <summary>
		/// Handles value change of ItemSizeRatio property
		/// </summary>
		private static void OnItemSizeRatioChanged(ListView listView, double itemSizeRatio)
		{
			SubscribeToSizeChanged(listView, GetNumberOfColumns(listView), itemSizeRatio, GetMarginBetweenColumns(listView));
		}
		#endregion

		#region MarginBetweenColumns
		/// <summary>
		/// Get the MarginBetweenColumns
		/// </summary>
		public static double GetMarginBetweenColumns(DependencyObject obj)
		{
			return (double)obj.GetValue(MarginBetweenColumnsProperty);
		}

		/// <summary>
		/// Set the MarginBetweenColumns
		/// </summary>
		public static void SetMarginBetweenColumns(DependencyObject obj, double value)
		{
			obj.SetValue(MarginBetweenColumnsProperty, value);
		}

		/// <summary>
		/// Exposes MarginBetweenColumns property
		/// </summary>
		public static readonly DependencyProperty MarginBetweenColumnsProperty =
			DependencyProperty.RegisterAttached("MarginBetweenColumns", typeof(double), typeof(ListViewBaseMultiColumnExtension), new PropertyMetadata(0d, (d, e) => OnMarginBetweenColumnsChanged((ListView)d, (double)e.NewValue)));

		/// <summary>
		/// Handles value change of MarginBetweenColumns property
		/// </summary>
		private static void OnMarginBetweenColumnsChanged(ListView listView, double marginBetweenColumns)
		{
			SubscribeToSizeChanged(listView, GetNumberOfColumns(listView), GetItemSizeRatio(listView), marginBetweenColumns);
		}
		#endregion

		/// <summary>
		/// Any of the property value change will trigger this method which will trigger Update the size of list view template
		/// This method also deregister and reregister OnGridViewSizeChanged method
		/// to the sizeChanged event to make sure there is always only one event handler responding to ListView Size Change
		/// </summary>
		private static void SubscribeToSizeChanged(ListView listView, int numberOfColumns, double itemSizeRatio, double marginBetweenColumns)
		{
			if (numberOfColumns <= 0 || itemSizeRatio <= 0)
			{
				return;
			}

			if (listView.Height > 0 && listView.Width > 0)
			{
				Update(listView, numberOfColumns, itemSizeRatio, marginBetweenColumns);
			}

			listView.SizeChanged -= OnListViewSizeChanged;
			listView.SizeChanged += OnListViewSizeChanged;
		}

		/// <summary>
		/// ListView SizeChanged event handler will call this method to update if listview size is changed
		/// </summary>
		private static void OnListViewSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var list = sender as ListView;

			Update(list, GetNumberOfColumns(list), GetItemSizeRatio(list), GetMarginBetweenColumns(list));
		}

		/// <summary>
		/// The method to calculate and update the new size of listview template item
		/// </summary>
		private static void Update(ListView listView, int numberOfColumns, double itemSizeRatio, double marginBetweenColumns)
		{
			var itemsWrapGrid = listView.ItemsPanelRoot as ItemsWrapGrid;

			var totalWidth = listView.ActualWidth;
			var columnWidth = Math.Floor((totalWidth - (marginBetweenColumns * (numberOfColumns - 1)) - listView.Padding.Left - listView.Padding.Right) / numberOfColumns);

			itemsWrapGrid.ItemWidth = columnWidth;
			itemsWrapGrid.ItemHeight = columnWidth * itemSizeRatio;
		}
	}
}
#endif
