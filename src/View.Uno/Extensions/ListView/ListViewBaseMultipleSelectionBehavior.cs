#if WINDOWS_UWP || __ANDROID__ || __IOS__
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using Uno.Extensions;
using Uno.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	/// <summary>
	/// Behavior that allows values to be bound to the SelectedItems property of a ListViewBase control
	/// </summary>
	public class ListViewBaseMultipleSelectionBehavior
	{
		private static readonly Lazy<ILogger> _log = new Lazy<ILogger>(() => typeof(ListViewBaseMultipleSelectionBehavior).Log(), true);

		#region IsSelectionChanging Private Attached Property

		private static bool GetIsSelectionChanging(ListViewBase list)
		{
			return (bool)list.GetValue(IsSelectionChangingProperty);
		}

		private static void SetIsSelectionChanging(ListViewBase list, bool value)
		{
			list.SetValue(IsSelectionChangingProperty, value);
		}

		private static readonly DependencyProperty IsSelectionChangingProperty =
			DependencyProperty.RegisterAttached("IsSelectionChanging", typeof(bool), typeof(ListViewBaseMultipleSelectionBehavior), new PropertyMetadata(false));

		#endregion

		#region IsAttached Private Attached Property
		private static bool GetIsAttached(ListViewBase list)
		{
			return (bool)list.GetValue(IsAttachedProperty);
		}

		private static void SetIsAttached(ListViewBase list, bool value)
		{
			list.SetValue(IsAttachedProperty, value);
		}

		private static readonly DependencyProperty IsAttachedProperty =
			DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(ListViewBaseMultipleSelectionBehavior), new PropertyMetadata(null, (d, args) => OnIsAttachChanged((ListViewBase)d, (bool)args.NewValue)));

		[SuppressMessage("High cyclomatic complexity", "NV2005", Justification = "Readable")]
		private static void OnIsAttachChanged(ListViewBase list, bool isAttached)
		{
			if (isAttached)
			{
				if (list.SelectionMode != ListViewSelectionMode.Multiple)
				{
					typeof(ListViewBaseMultipleSelectionBehavior).Log().DebugIfEnabled(() => "ListViewBase using MultiSelectionBehaviour is not using SelectionMode.Multiple");
				}

				Observable
					.FromEventPattern<SelectionChangedEventHandler, SelectionChangedEventArgs>(
						d => list.SelectionChanged += d,
						d => list.SelectionChanged -= d,
						new MainDispatcherScheduler(list.Dispatcher)
					)
					.SubscribeToElement(
						list,
						_ => OnSelectedItemsChanged(list, list.SelectedItems?.ToArray(), new object[0]),
						ex => typeof(ListViewBaseMultipleSelectionBehavior).Log().Error("An error occurred while subscribing to ListViewBase.SelectionChanged", ex),
						() => SetIsAttached(list, false)
					);
			}
		}
		#endregion

		#region SelectedItems Property
		public static object[] GetSelectedItems(DependencyObject obj)
		{
			return (object[])obj.GetValue(SelectedItemsProperty);
		}

		public static void SetSelectedItems(DependencyObject obj, object[] value)
		{
			obj.SetValue(SelectedItemsProperty, value);
		}

		public static readonly DependencyProperty SelectedItemsProperty =
			DependencyProperty.RegisterAttached("SelectedItems", typeof(object[]), typeof(ListViewBaseMultipleSelectionBehavior), new PropertyMetadata(null, (d, args) => OnSelectedItemsChanged((ListViewBase)d, (object[])args.NewValue, (object[])args.OldValue)));

		private static void OnSelectedItemsChanged(ListViewBase list, object[] newSelectedItems, object[] oldSelectedItems)
		{
			if (Enumerable.Equals(newSelectedItems, oldSelectedItems))
			{
				return;
			}

			if (!GetIsAttached(list))
			{
				SetIsAttached(list, true);
			}

			if (!GetIsSelectionChanging(list))
			{
				SetIsSelectionChanging(list, true);

				try
				{
					if (newSelectedItems != null)
					{
						list.SelectedItems.ReplaceWith(newSelectedItems);
						SetSelectedItems(list, newSelectedItems);
					}
				}
				finally
				{
					SetIsSelectionChanging(list, false);
				}
			}
		}
		#endregion
	}
}
#endif
