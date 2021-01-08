# ListViewDismissKeyboardBehavior

## Summary

The `ListViewDismissKeyboardBehavior` can be used when there is a TextBox and ListView on the same page to ensure the software keyboard is dismissed whenever the ListViewBase is scrolled.

## Platform support

| Feature                                                                                         | UWA | Android | iOS |
| ----------------------------------------------------------------------------------------------- |:---:|:-------:|:---:|
| Dismiss software keyboard whenever the ListViewBase is scrolled                                 |     |    X    |  X  |


## Usage

- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:

    ```xml
			<ListView 
				ue:ListViewDismissKeyboardBehavior.IsEnabled="True"/>
    ```


	
## Known issues
None.
