# SwipeRefresh

## Summary

The `SwipeRefresh` control implements the **swipe to refresh** pattern.

1. The user swipes from the top of the control.

1. A refresh is triggered to get the latest content.

1. A **progress indicator** shows the progress of the refresh.

The expected behavior for **swipe to refresh** varies per platform.

- On [Android](https://material.google.com/patterns/swipe-to-refresh.html), the **progress indicator** appears  over the content. 

- On [iOS](https://developer.apple.com/ios/human-interface-guidelines/ui-controls/refresh-content-controls/), the content scrolls down to reveal the **progress indicator**.

## Platform support

| Feature                                    | UWA | Android | iOS |
| ------------------------------------------ |:---:|:-------:|:---:|
| Swipe to refresh a `ListView`             |  X  |    X    |  X  |
| Swipe to refresh any `Control`            |  X  |    X    |  X  |
| Swipe to refresh an `AsyncValuePresenter` |  X  |    X    |  X  |
| Specify the refresh command                |  X  |    X    |  X  |
| Specify the progress indicator color       |  X  |    X    |  X  |
| Specify additional indicator colors        |  -  |    X    |  -  |
| Specify the progress indicator template    |  X  |    -    |  -  |
| Specify the progress indicator offset      |  -  |    X*    |  X  |
| Specify scrollviewers to "opt-out"         |  -  |    X    |  -  |
\*Android progress indicator offset only supports Y offset

## Usage

The `SwipeRefresh` control requires your content to be be inside a `ScrollViewer`.

```xml
<SwipeRefresh>
	<ScrollViewer>
		<!-- put your content here -->
	</ScrollViewer>
</SwipeRefresh>
```

In the case of a `ListViewBase`, don't wrap your `ListViewBase` inside a `ScrollViewer`. This special case is handled automatically by the `SwipeRefresh` control.

```xml
<SwipeRefresh>
	<ScrollViewer>
		<ListView />
	</ScrollViewer>
</SwipeRefresh>
```

A `Command` is executed once the **swipe to refresh** gesture is completed. The command can be binded using the `RefreshCommand` property on the `SwipeRefresh` control.

```xml
<u:SwipeRefresh RefreshCommand="{Binding [MyRefreshCommand]}">
...
</u:SwipeRefresh>
```

To complete the **progress indicator**, you must set the `IsRefreshing` property to `false` on the `SwipeRefresh` control.

```xml
<u:SwipeRefresh RefreshCommand="{Binding [MyRefreshCommand]}"
				IsRefreshing="{Binding [IsRefreshing], Mode=TwoWay}">
...
</u:SwipeRefresh>
```

```csharp
IsRefreshing.Value.OnNext(false);
```

You have the option to change the color of the **progress indicator**.

```xml
<u:SwipeRefresh RefreshCommand="{Binding [MyRefreshCommand]}"
				IsRefreshing="{Binding [IsRefreshing], Mode=TwoWay}"
				IndicatorColor="Green">
...
</u:SwipeRefresh>
```

Each platform uses a different template. Look at the [SwipeRefresh.xaml](SwipeRefresh.xaml) file to get the proper styles.

On Android, if a SwipeRefresh control contains more than one scrollable control (ScrollViewer, ListViewBase) in its visual tree, you can use the `IncludeInSwipeRefresh` attachable property to not affect the Pull-To-Refresh behavior by setting it to `False`.

```xml
<DataTemplate x:Key="TheatreShowtimesEmptyTemplate">
	<ScrollViewer u:SwipeRefresh.IncludeInSwipeRefresh="False">
		<u:StarStackPanel>
			.
			.
			.
		</u:StarStackPanel>
	</ScrollViewer>
</DataTemplate>
```

By default, `IncludeInSwipeRefresh` is set to `True`. Setting it to `False` will cause the SwipeRefresh control on Android to skip the attached control during it's lookup when searching for a child scrollable.

## Known issues

None.