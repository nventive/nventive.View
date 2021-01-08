# ExpandIntoOverscrollBehavior

## Summary

`ExpandIntoOverscrollBehavior` implements iOS-specific functionality: when a `ScrollViewer` or `ListView` is 'stretched' past its 
origin (eg as in a swipe-refresh gesture), the tagged content will expand into the available space. This can be more visually pleasing 
than seeing the background behind the `ScrollViewer`.

## Platform support

| Feature                               | UWA | Android | iOS |
| --------------------------------------|:---:|:-------:|:---:|
| Expand contents to fill 'overscroll'  |     |         |  X  |

## Usage

```xml
<ScrollViewer>
	<StackPanel>
		<Image ue:ExpandIntoOverscrollBehavior.IsEnabled="True" />
		<TextBlock Text="It was the best of times, it was the blurst of times..."/>
	</StackPanel>
</ScrollViewer>
```

In this case the Image will expand to fill the empty space when overscrolling.

```xml
<ListView >
			<ListView.HeaderTemplate>
				<DataTemplate>
					<StackPanel ue:ExpandIntoOverscrollBehavior.IsEnabled="True">
						<Image />
						<TextBlock />
					</StackPanel>
				</DataTemplate>
			</ListView.HeaderTemplate>
			...
		</ListView>
```

In the above case, the StackPanel and its contents will expand into the overscroll.

*Note:* The behavior can be safely set on UWP and Android and will compile, it just won't do anything.

## Known issues

The behavior disables `ClipsToBounds` on views between the target element and the `ScrollViewer` in the visual tree. For most 
views in the Uno framework this shouldn't affect the display appearance.
