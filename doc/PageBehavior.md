# PageBehavior

## Summary

The `PageBehavior` regroups utility attached properties for the Page or UserControl classes.

### StatusBarVisibility
You can set `Visible` or `Collapsed`. If you hide the status bar on a page, it will stay hidden until another
page explicitly sets the property to `Visible`. You could also bind the visibility.

### StatusBarForegroundColor
The property is a `Color`. However there are platform and OS limitations. On iOS and Android, the color will be converted to black or white.
On some versions of Android, the status bar is always white on a dark background.

## Platform support

| Feature                       | UWA | Android | iOS |
| ----------------------------- |:---:|:-------:|:---:|
| StatusBarVisibility           |  X  |    X    |  X  |
| StatusBarForegroundColor      |  X  |    X    |  X  |
| StatusBarBackgroundColor      |  -  |    -    |  -  |
| StatusBarBackgroundOpacity    |  -  |    -    |  -  |


## Usage

- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:

   ```xml
	<Page x:Class="RxTx.Views.Content.ContentOptionsPage"
		  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		  xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		  xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		  xmlns:ue="using:Umbrella.View.Extensions"
		  xmlns:uloc="http://nventive.com/localization/1.0"
		  mc:Ignorable="d uloc"
		  ue:PageBehavior.StatusBarVisibility="Visible"
		  ue:PageBehavior.StatusBarForegroundColor="White">
    ```
	
## Known issues

None.
