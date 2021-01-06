# ScrollViewerExtensions.ScrollIndicatorStyle

## Summary

The `ScrollViewerExtensions.ScrollIndicatorStyle` allows for setting the scroll bar style on iOS devices. Apple provides us with three different styles, and it is not possible to customize them. See [the Apple documentation](https://developer.apple.com/documentation/uikit/uiscrollview/indicatorstyle) for more detail on the specific styles.

> **Default** - The default style of scroll indicator, which is black with a white border. This style is good against any content background.

> **Black** - A style of indicator which is black and smaller than the default style. This style is good against a white content background.

> **White** - A style of indicator is white and smaller than the default style. This style is good against a black content background.

This works with ScrollViewer and ListView.

## Platform support

| Feature                           | UWA | Android | iOS |
| --------------------------------- |:---:|:-------:|:---:|
| Set the scroll indicator style    |     |         |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:
    ```xml
    <ScrollViewer ue:ScrollViewerExtensions.ScrollIndicatorStyle="White">
		...
    </ScrollViewer>
    ```
	
## Known issues
None.
