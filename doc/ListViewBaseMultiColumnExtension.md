# ListViewBaseMultiColumnExtension

## Summary

The `ListViewBaseMultiColumnExtension` is a class that exposes attachable properties `NumberOfColumns`, `ItemSizeRatio`, and `MarginBetweenColumns` to ListViewBase control. By setting these properties, program can automatically update the listview item template's size when the size of the screen changes, keeping the NumberOfColumns, ItemSizeRatio, and MarginBetweenColumns as defined.

## Platform support

| Feature                     | UWA | Android | iOS |
| --------------------------- |:---:|:-------:|:---:|
| NumberOfColumns             |  X  |    X    |  X  |
| ItemSizeRatio               |  X  |    X    |  X  |
| MarginBetweenColumns        |  X  |    X    |  X  |

## Usage

Add the Umbrella View Extensions namespace in the XAML namespace section of your Page/UserControl:

```xml
xmlns:ue="using:Umbrella.Views.Extensions"
```
On a ListViewBase control (ListView, GridView and etc), add at least `NumberOfColumns` ("1", "2", "3" etc) and `ItemSizeRatio` attached propeties to the Xaml file. Add also `ItemsPanel` property where we need to define a ItemsPanelTemplate with `<ItemsWrapGrid Orientation="Horizontal" />`.

**ItemSizeRatio**

This property is calculating the width of the listview items through the `ListViewBase`'s Size and then calculating the Height based on ItemSizeRatio.
For example `ItemSizeRatio="1.0"` would yield square items and `ItemSizeRatio="1.5"` would display tall items

- In the XAML:

    ```xml
		    <ListView ue:ListViewBaseMultiColumnExtension.NumberOfColumns="3"
				      ue:ListViewBaseMultiColumnExtension.ItemSizeRatio="1.50">

			    <ListView.ItemsPanel>
				    <ItemsPanelTemplate>
					    <ItemsWrapGrid Orientation="Horizontal" />
				    </ItemsPanelTemplate>
			    </ListView.ItemsPanel>
    ```

NOTE: **Do not** define `ItemWidth`, `ItemHeight`, `MaximumRowsOrColumns` to ItemsWrapGrid.

## Known issues

When changing the orientation on the phone, the list view item size(may show 3 columns of items even if you defined 2 columns) in the screen will not be updated, while other items outside the screen will be updated. However, if you scroll down/up and then scroll back, the initial screen you were looking at right after changing orientation will be updated as normal.
Same issue happens when first loading to the page when using AVP, since it is a transition from other template to the data template with MultiColumnListViewExtension.
