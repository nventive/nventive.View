#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if WINDOWS_UWP
using IDependencyObject = Windows.UI.Xaml.DependencyObject;
#endif


namespace Nventive.View.Extensions
{
	/// <summary>
	/// A behavior that, when attached to a listView, scrolls SelectedItem into view when it is selected.
	/// </summary>
	public partial class ListViewBaseExtensions
	{
		/*
		 * Developer notes:
		 * 
		 * Using our own enum instead of the Windows ScrollIntoViewAlignment because we need more than 2 values
		 * If we have only 2 values (Default and Leading) and initialize the property to Default, we won't pass in the OnChanged Handler if Default is set in XAML.
		 */

		/// <summary>
		/// Options of BringIntoView Alignments with a default value of None,
		/// to be usable in AttachedProperties and have OnPropertyChanged properly called
		/// </summary>
		public enum BringIntoViewAlignment
		{
			/// <summary>
			/// No bring into view is executed
			/// </summary>
			None = 0,

			/// <summary>
			/// If the item is outside the ViewPort, it will be brought in to the closest edge. No effect occurs if the item is inside the viewport.
			/// </summary>
			ClosestEdge,

			/// <summary>
			/// The item is brought to the leading edge of the ViewPort.
			/// </summary>
			Leading,
		}

		public static BringIntoViewAlignment GetBringSelectedItemIntoViewAlignment(DependencyObject obj)
		{
			return (BringIntoViewAlignment)obj.GetValue(BringSelectedItemIntoViewAlignmentProperty);
		}

		/// <summary>
		/// Set the IsEnabled attached property to true to activate the behavior.
		/// </summary>
		public static void SetBringSelectedItemIntoViewAlignment(DependencyObject obj, BringIntoViewAlignment value)
		{
			obj.SetValue(BringSelectedItemIntoViewAlignmentProperty, value);
		}

		public static readonly DependencyProperty BringSelectedItemIntoViewAlignmentProperty =
			DependencyProperty.RegisterAttached("BringSelectedItemIntoViewAlignment", typeof(BringIntoViewAlignment), typeof(ListViewBaseExtensions), new PropertyMetadata(BringIntoViewAlignment.None, OnBringSelectedIntoViewAlignmentChanged));

		private static void OnBringSelectedIntoViewAlignmentChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var listView = (Windows.UI.Xaml.Controls.ListViewBase)d;

			if ((BringIntoViewAlignment)e.NewValue == BringIntoViewAlignment.None)
			{
				listView.SelectionChanged -= OnListViewSelectionChanged;
			}
			else
			{
				listView.SelectionChanged -= OnListViewSelectionChanged;
				listView.SelectionChanged += OnListViewSelectionChanged;
			}
		}

		private static void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listView = (Windows.UI.Xaml.Controls.ListViewBase)sender;
			var selectedItem = listView.SelectedItem;

			if (selectedItem != null)
			{
				listView.ScrollIntoView(selectedItem, ToScrollIntoViewAlignment(GetBringSelectedItemIntoViewAlignment(listView)));
			}
		}

		private static ScrollIntoViewAlignment ToScrollIntoViewAlignment(BringIntoViewAlignment alignment)
		{
			switch (alignment)
			{
				case BringIntoViewAlignment.Leading:
					return ScrollIntoViewAlignment.Leading;
				case BringIntoViewAlignment.None:
				case BringIntoViewAlignment.ClosestEdge:
				default:
					return ScrollIntoViewAlignment.Default;
			}
		}
	}
}
	#endif
