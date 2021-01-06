# ProportionalPanel

## Summary

The `ProportionalPanel` is a panel which maintains proportion on the child controls. The user can set the property `Mode` to either "HeightFollowsWidth" or "WidthFollowsHeight" and define `Ratio`.
As a result, the Panel will always maintain the proportion set and the child controls will get resized with panel in the same manner.

## Platform support

| Feature                                    | UWA | Android | iOS |
| ------------------------------------------ |:---:|:-------:|:---:|
| Maintains proportion on the child controls |  X  |    X    |  X  |

## Usage

Add the Umbrella View Controls namespace in the XAML namespace section of your Page/UserControl:

```xml
xmlns:u="using:Umbrella.View.Controls"
```

- In the XAML:

    ```xml
	<u:ProportionalPanel
        Mode="HeightFollowsWidth"
        Ratio="2.5">
		<u:ImagePresenter
            ImageSource="{Binding [Url]}"
			Stretch="UniformToFill" />
	</u:ProportionalPanel>
    ```

The user can set the property `Mode` to either "HeightFollowsWidth" or "WidthFollowsHeight" and define `Ratio`. Other common panel properties can also be set.
When "HeightFollowsWidth" is set, this property takes available width of the panel and calculates the height according to ratio, regardless of the height set at the property.
When "WidthFollowsHeight" is set, this property takes available height of the panel and calculates the width according to ratio, regardless of the width set at the property.

**Ratio**

This property defines the "width/height" ratio. (Careful! The `ItemSizeRatio` in ListViewBaseMultiColumnExtension is "height/width".)
For example `Ratio="1.0"` would yield a square panel and `Ratio="0.5"` would display a tall panel.

## Known issues

When using the ProportionalPanel inside a ListView item, "NSInternalInconsistencyException" will appear when deploying on iOS devices. The reason is ProportionalPanel tries to get the default ListView item size. And the ListViewBaseSource.iOS.cs implementation in UNO returns "double.MaxValue" size for iOS ListView item. The "double.MaxValue" is not acceptable for ProportionalPanel implementation. However, UWP and Android will not encounter the same error as their native ListView item size is returned based the screen size.

There are three workarounds:

* Apply the ListViewBaseMultiColumnExtension to the ListView. The extension will determine the ListView item size before ProportionalPanel start to render. See the [ListViewBaseMultiColumnExtension](ListViewBaseMultiColumnExtension.md)

##### Example

```XML
<ListView
    Grid.Row="1"
    ItemsSource="{Binding [Items]}"
    SelectedItem="None"
    ScrollViewer.IsVerticalRailEnabled="False"
    ue:ListViewBaseMultiColumnExtension.NumberOfColumns="1"
    ue:ListViewBaseMultiColumnExtension.ItemSizeRatio="1.50">

	<ListView.ItemTemplate>
		<DataTemplate>
			<Grid Padding="0,0,0,20">
				<u:ProportionalPanel
                    Mode="WidthFollowsHeight"
                    Ratio="1.0">
					<u:ImagePresenter
                        ImageSource="{Binding Url}"
                        Stretch="UniformToFill" />
				</u:ProportionalPanel>
			</Grid>
		</DataTemplate>
	</ListView.ItemTemplate>
</ListView>
```

* Set Width to ProportionalPanel if the `Mode` is "HeightFollowsWidth". Or set Height to ProportionalPanel if the `Mode` is "WidthFollowsHeight".

##### Example

```XML
<u:ProportionalPanel
    Mode="WidthFollowsHeight"
    Ratio="1.0"
    Height="300">
	<u:ImagePresenter
        ImageSource="{Binding Url}"
        Stretch="UniformToFill" />
</u:ProportionalPanel>
```

* Set `ItemWidth` **and** `ItemHeight` to ItemWrapGrid to limit the ListView item size.

##### Example

```XML
<ListView.ItemsPanel>
	<ItemsPanelTemplate>
		<ItemsWrapGrid
            ItemWidth="300"
            ItemHeight="300"/>
	</ItemsPanelTemplate>
</ListView.ItemsPanel>
```
