
# ParallaxListView

## Summary

The `ParallaxListView` adds a customizable header to a ListView and a parallax effect when scrolling.

### HeaderBackgroundTemplate
Sets the header's background displayed above the `ListView` items. It is displayed within a `ContentPresenter` with both the `VerticalAlignment` and `HorizontalAlignment` properties set to `Stretch` in the default style.

### HeaderHeight
The property is a `Double`. It sets the header's height. If you wish for the header to fill the whole page, you could bind the property to `Windows.UI.Xaml.Window.Current.Bounds.Height` to get the full window.

### ItemCommand
The command executed when the user taps on an item within the list.

### ItemContainerStyle
The `ListView` item container style.

### ItemsSource
The `ListView` item source.

### ItemTemplate
The `ListView` item template.

### ItemTemplateSelector
The `ListView` item template selector.

### IsNonInteractiveHeaderForegroundFading
The property is set to `false` by default. If enabled, the `NonInteractiveHeaderForeground` content will fade as part of the parallax effect when the user scrolls. 

### NonInteractiveHeaderForegroundTemplate
Sets the header's non interactive foreground. It is displayed within a `ContentPresenter` with both the `VerticalAlignment` and `HorizontalAlignment` properties set to `Stretch` in the default style.

### ScrollStatus
The property is a `string`. It allows to observe the status of the scroll if you would like to adapt other UI elements based on it. For example it could allow for a commandbar to be hidden based on this property.  If you wish for the header to fill the whole page, you should hide the default command bar using this property.

## Platform support

| Feature                                            | UWA | Android | iOS |
| -------------------------------------------------- |:---:|:-------:|:---:|
| ListView & Customizable Header with Parallax Effect|  X  |    X    |  X  |

## Usage

	<u:ParallaxListView ItemsSource="{Binding [Items]}"
						ItemTemplate="{StaticResource MockItemTemplate}"
						HeaderHeight="500"
						NonInteractiveHeaderForegroundTemplate="{StaticResource MockHeaderForegroundTemplate}"
						HeaderBackgroundTemplate="{StaticResource MockHeaderBackgroundTemplate}" />

## Known issues

None.
