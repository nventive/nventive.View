# TextBoxBehavior

## Summary

The `TextBoxBehavior` class regroups multiple behaviors for the TextBox class.
Some of those behaviors are also available for the PasswordBox class through the `PasswordBoxBehavior` class.

### NextControl
The `NextControl` property allows you to specify what `Control` will be focused next when the user presses the *Enter* button of the keyboard.

### EnterCommand
The `EnterCommand` property allows you to bind an `ICommand` to the *Enter* button of the keyboard.
Note that you can't specify a command parameter with this.
Also note that according to the latest guidelines, you should not use this property.

### ActiveCommand
The `ActiveCommand` property allows you to bind an `ActiveCommand` to the *Enter* button of the keyboard. Note that you can't specify a command parameter with this.

### ActiveButton
The `ActiveButton` property allows you to set an `ActiveButton`'s `ActiveCommand` to the current `TextBox`'s `ActiveCommand`.
That means you also have to set the `ActiveCommand` property of this class in order to use this.
This is usefull when you want the button to be disabled when executing the `ActiveCommand` from the *Enter* button of the keyboard.

### AutoInputFormat
The `AutoInputFormat` property allows you to specify a specific input format to the `TextBox`.
This is an enum, here are the possible values:
- `None`: Default value, nothing happens.
- `DecimalNumber`: You can only type a numerical integer or decimal value. (1, 2, 3.5, 45, etc.)
The digit separator (if any) will be localized (e.g. ',' in French, '.' in English).
Negative values are not supported.

### DismissKeyboardOnEnter
The `DismissKeyboardOnEnter` property allows you to specify whether the soft keyboard should closes itself when you press the *Enter* button of the keyboard.
Note that this property is only relevant on Android because both UWA and iOS dismiss the keyboard automatically.

## Platform support

| Feature                |UWA|Android|iOS|
| -----------------------|:-:|:-:|:-:|
| NextControl            | X | X | X |
| EnterCommand		     | X | X | X |
| ActiveCommand		     | X | X | X |
| ActiveButton           | X | X | X |
| AutoInputFormat to IntegerNumber | - | - | - |
| AutoInputFormat to DecimalNumber | X | - | - |
| DismissKeyboardOnEnter | X | X | X |

## Usage

Add the Umbrella behaviors namespace in the XAML namespace section of your Page/UserControl:

```xml
xmlns:ue="using:Umbrella.View.Extensions"
```

The following example uses all of the mentionned behaviors.
- The first TextBox will only accept numbers as you type and will pass focus to the second TextBox when pressing *Enter*.
- The second TextBox will execute an `ICommand` and pass focus to the third one when pressing *Enter*.
- The third TextBox will execute an `ActiveCommand` and dismiss the keyboard when pressing *Enter*. It's also associated with an `ActiveButton`.  

```xml	
<TextBox x:Name="FirstTextBox"
	ue:TextBoxBehavior.NextControl="{Binding ElementName=SecondTextBox}"
	ue:TextBoxBehavior.AutoInputFormat="NumberWithDigitSeparator" />

<TextBox x:Name="SecondTextbox"
	ue:TextBoxBehavior.NextControl="{Binding ElementName=ThirdTextBox}"
	ue:TextBoxBehavior.EnterCommand="{Binding [MyCommand]}" />

<TextBox x:Name="ThirdTextBox"
	ue:TextBoxBehavior.ActiveEnterCommand="{Binding [MyActiveCommand]}"
	ue:TextBoxBehavior.ActiveButton="{Binding ElementName=ActiveButtonSuccess}"
	ue:TextBoxBehavior.DismissKeyboardOnEnter="True" />

<u:ActiveButton x:Name="ActiveButtonSuccess"
	ActiveCommand="{Binding [MyActiveCommand]}" />
```
	
## Known issues

- The `AutoInputFormat` property requires `TextChanging` event of `TextBox` and it's not implemented in Uno.UI.
- If you type at a custom cursor position (not the end) when using `AutoInputFormat`, the cursor moves to the end.
