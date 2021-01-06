# InputAccessoryViewBehavior

## Summary

The `InputAccessoryViewBehavior` behavior adds the InputAccessoryView (toolbar) on top of the `[iOS]` keyboard.

1. There must be a **unique unmutable `ID` (string)** for each TextBox using the toolbar, otherwise you will not get the expected behavior.

2. The labels are **strings**.

3. Labels must be provided for the buttons needed. If omitted, the button will not show. If a command is changed but a label is not provided, the button will not show.

4. The default command for the `Previous` button is to go to the previous focusable element.

5. The default command for the `Next` button is to go to the next focusable element.

6. The default command for the `Done` button is to close the keyboard. If the command is changed, clicking on the button will execute the new command if it can and then close the keyboard.

7. The buttons are displayed as such:
**Previous | Next | (flexibleSpace) | Done**

## Platform support

| Feature                           | UWA | Android | iOS |
| --------------------------------- |:---:|:-------:|:---:|
| `PreviousLabel`                   |  -  |    -    |  X  |
| PreviousCommand                   |  -  |    -    |  X  |
| `NextLabel`                       |  -  |    -    |  X  |
| NextCommand                       |  -  |    -    |  X  |
| `DoneLabel`                       |  -  |    -    |  X  |
| DoneCommand                       |  -  |    -    |  X  |
| **`ID`**                          |  -  |    -    |  X  |

## Usage

The `InputAccessoryViewBehavior` requires the `ios:` prefix to be used. It is located in Umbrella Behaviors.

1. The labels can be set directly in the XAML.
2. The labels can be binded to a value in the ViewModel.
3. The labels can be binded to a mutable value and the text on the button will change while the keyboard is open.

```xml
    xmlns:ios="http:///uno/ui/ios"

    [...]
    <TextBox ios:InputAccessoryViewBehavior.PreviousLabel="Previous"
             ios:InputAccessoryViewBehavior.NextLabel="{Binding [NextLabel]}"
             ios:InputAccessoryViewBehavior.ID="Second"
             ios:InputAccessoryViewBehavior.DoneLabel="{Binding [TextThirdBox], Mode=TwoWay}"
             ios:InputAccessoryViewBehavior.DoneCommand="{Binding [DoneCommand]}"
             [...] />
    [...]
```


## Known issues

The `Previous` default behavior does not bring you to the previous focusable element in the visual tree at the moment. 

```csharp
    FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);
```

The `FocusManager` does not seem to find the right element.

Look at the [***UserEcho***](http://feedback.nventive.com/topics/1053-/) for updates on the issue.
