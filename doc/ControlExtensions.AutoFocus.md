# ControlExtensions.AutoFocus

## Summary

The `ControlExtensions.AutoFocus` allows control to receive the focus automatically once they are loaded. 
Focus on `TextBox` and `PasswordBox` brings the keyboard up.
For `TextBox` the caret will be at the end if there is already some content.

## Platform support

| Feature                                                                                         | UWA | Android | iOS |
| ----------------------------------------------------------------------------------------------- |:---:|:-------:|:---:|
| Assign focus to the element everytime the page is loaded                                        |  X  |    X    |  X  |
| Assign focus to the element only the first time the page is loaded                              |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:
    ```xml
		
		<TextBox ue:ControlExtensions.IsAutoFocus="True" />

		<TextBox ue:ControlExtensions.IsAutofocusingOnce="True" />

    ```
	
## Known issues
None.
