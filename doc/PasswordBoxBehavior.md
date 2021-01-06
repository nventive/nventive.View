# PasswordBoxBehavior

## Summary

The `PasswordBoxBehavior` allows PasswordBox to react to keyboard events like pressing next or done.

## Platform support

| Feature                                                                                         | UWA | Android | iOS |
| ----------------------------------------------------------------------------------------------- |:---:|:-------:|:---:|
| Assign next control to get the focus when user press the "Next" button on the keyboard          |  X  |    X    |  X  |
| Assign command to be called for when the user presses the "Enter" button on the keyboard        |  X  |    X    |  X  |
| Assign ActiveCommand to be called for when the user presses the "Enter" button on the keyboard  |  X  |    X    |  X  |
| Assign activeButton that will trigger the EnterActiveCommand when clicked                       |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:

    ```xml
		
		<PasswordBox x:Name="FirstPasswordBox"
			ue:TextBoxBehavior.NextControl="{Binding ElementName=SecondPasswordBox}" />
		
		<PasswordBox x:Name="SecondTextbox"
			ue:TextBoxBehavior.NextControl="{Binding ElementName=ThirdPasswordBox}"
			ue:TextBoxBehavior.EnterCommand="{Binding ContextualCommand}" />

		<PasswordBox x:Name="ThirdPasswordBox"
			ue:PasswordBoxBehavior.ActiveEnterCommand="{Binding [SampleCommand]}"
			ue:PasswordBoxBehavior.ActiveButton="{Binding ElementName=ActiveButtonSuccess}"  />

		<u:ActiveButton x:Name="ActiveButtonSuccess"
			ActiveCommand="{Binding [SampleCommand]}"
			CommandParameter="success" />
    ```
	
## Known issues

None.
