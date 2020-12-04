#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if HAS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif
#if __ANDROID__ || __IOS__
using _ListViewBase = Windows.UI.Xaml.DependencyObject;
#if __IOS__
using _ListViewLegacy = Uno.UI.Controls.Legacy.ListViewBase;
#else
using _ListViewLegacy = Uno.UI.Controls.Legacy.ListView;
#endif
#elif HAS_WINUI
using _ListViewBase = Microsoft.UI.Xaml.Controls.ListViewBase;
#else
using _ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;
#endif

namespace Chinook.View.Extensions
{
	/// <summary>
	/// Wraps a IGrouping<TKey, TValue> into a CollectionViewSource with IsSourceGrouped enabled. Can be used with IEnumerable.GroupBy or GroupAlphabetically.
	/// </summary>
	public class GroupedCollectionBehavior
	{
		public static object GetItemsSource(_ListViewBase depobject)
		{
			return depobject.GetValue(ItemsSourceProperty);
		}

		public static void SetItemsSource(_ListViewBase depobject, object source)
		{
			depobject.SetValue(ItemsSourceProperty, source);
		}

		public static readonly DependencyProperty ItemsSourceProperty =
				DependencyProperty.RegisterAttached(
					"ItemsSource",
					typeof(object),
					typeof(GroupedCollectionBehavior),
					new PropertyMetadata(null, (d, e) => new Builder((_ListViewBase)d).UpdateSource())
				);

		class Builder
		{
			private readonly _ListViewBase _listViewBase;

			public Builder(_ListViewBase d)
			{
				this._listViewBase = d;
			}

			public void UpdateSource()
			{
				var source = GetItemsSource(_listViewBase);

				if (source == null)
				{
#if WINDOWS_UWP || HAS_WINUI
					_listViewBase.ItemsSource = null; 
#else
					if (_listViewBase is ListViewBase listViewBase)
					{
						listViewBase.ItemsSource = null;
					}
					else if (_listViewBase is _ListViewLegacy legacyListViewBase)
					{
						legacyListViewBase.ItemsSource = null;
					}
#endif

				}
				else
				{
					var viewSource = new CollectionViewSource()
					{
						Source = source,
						IsSourceGrouped = true
					};

#if WINDOWS_UWP || HAS_WINUI
					_listViewBase.ItemsSource = viewSource.View; 
#else
					if (_listViewBase is Windows.UI.Xaml.Controls.ListViewBase listViewBase)
					{
						listViewBase.ItemsSource = viewSource.View;
					}
					else if (_listViewBase is _ListViewLegacy legacyListViewBase)
					{
						legacyListViewBase.ItemsSource = viewSource.View;
					}
#endif
				}
			}
		}
	}
}

#endif
