# Expander

## Summary

The `Expander` is an expandable/collapsable control that displays a group of items, and can be toggled between states.
It is frequently used as a dropdown container for lists.

## Platform support

| Feature                         | UWA | Android | iOS |
| -----------------------------|--------|------------|------|
| DisplayHeader              |    X    |       X       |   X   |
| Open/Close on toggle |    X    |       X       |   X   |

## Usage

- In the namespace section:

    ```xml
    xmlns:u="using:Umbrella.View.Controls"
    ```
- In the XAML:

   ```xml
	<Page x:Class="RxTx.Views.Content.ContentOptionsPage"
		  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		  xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		  xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
          xmlns:u="using:Umbrella.View.Controls"
		  xmlns:uloc="http://nventive.com/localization/1.0"
		  mc:Ignorable="d uloc">

		  <u:Expander Header="{Binding}">
				<u:Expander.HeaderTemplate>
					<DataTemplate>
						<TextBlock Text="HeaderOfList" />
					</DataTemplate>
				</u:Expander.HeaderTemplate>
				<u:Expander.Content>
					<ItemsControl ItemsSource="ListItemsSource" />
				</u:Expander.Content>
			</u:Expander>
    ```
	
### Visual states

| VisualState name | VisualStateGroup name | Description                                                  |
|------------------------|---------------------------------|-----------------------------------------------------|
| `Opened`		          | `openStateGroup`            | The List of items is expanded/visible.
| `Closed`                 | `openStateGroup`            | The List of items is collapsed/not visible.

### Properties

HeaderTemplate: To customize the headers style
Header: Toggle button content usually name for item list group
PresenterHeight: to adjust height property of open state
CloseWhenUnloaded: bool for default expanded state on load
IsOpened: bool to identify/swap current state

## Known issues
None.
