# ControlExtensions.LostFocusBehavior

## Summary

`ControlExtensions.LostFocusBehavior` allows the execution of a Command when a `Control` loses focus, for example when tapping outside of a `TextBox`. 

It is possible to use this behavior on other components than `TextBox`, such as `Button` and `PasswordBox` even though that has very little use.

## Platform support

| Feature                                         | UWA | Android | iOS |
| ----------------------------------------------- |:---:|:-------:|:---:|
| Command triggers when a Control loses focus     |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:
    ```xml
	<TextBox ue:ControlExtensions.LostFocusCommand="{Binding [OnLostFocus]}" />
    ```
- In the ViewModel:
    ```csharp
    // In the constructor
    Build(b => b
        .Properties(pb => pb
            .AttachCommand("OnLostFocus", cb => cb.Execute(OnLostFocus))
        )
    );

    // In the class body
    private async Task OnLostFocus(CancellationToken ct) { }
    ```
	`OnLostFocus` will be called everytime the TextBox loses focus.

### (Optional) Using a CommandParameter
It is possible to provide a `CommandParameter` along with the lost focus Command. To do so, we need to:
- Specify the `CommandParameter` in the XAML:
    ```xml
    <TextBox ue:ControlExtensions.LostFocusCommand="{Binding [OnLostFocus]}"
             ue:ControlExtensions.LostFocusCommandParameter="Your parameter" />
    ```
- In the ViewModel:
    ```csharp
    private async Task<System.Reactive.Unit> OnParameteredLostFocus(object param, CancellationToken ct) 
    {
        // Write logic here...
        return null;
    }
    ```

### (Optional) Using CanExecute
It is possible to stop the Command from being executed by providing `CanExecute`.
- Provide `CanExecute` to the `CommandBuilder` in the ViewModel
    ```csharp
    Build(b => b
        .Properties(pb => pb
            .AttachCommand("OnLostFocus", cb => cb
                .Execute(OnLostFocus)
                .CanExecute(/* IObservable<bool> or method returning bool */)
            )
        )
    );
    ```


## Known issues
None.
