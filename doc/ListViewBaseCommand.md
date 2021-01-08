# ListViewBaseCommand

## Summary

The `ListViewBaseCommand` is a class that exposes an attachable property named `Command` that lets you bind an `ICommand` declared in a ViewModel to a `ListViewBase` control (*such as ListView, GridView*). This allows the click of each item in the list to execute the specified command.

The `ListViewBaseCommand` also exposes a `CommandParameter` attachable property which can be used to specify a parameter to use alongside the `ICommand` when executing.


## Platform support

| Feature                                               | UWA | Android | iOS |
| ----------------------------------------------------- |:---:|:-------:|:---:|
| Item click executes the ICommand                      |  X  |    X    |  X  |
| Pass a parameter using CommandParameter               |  X  |    -    |  -  |

## Usage

### Configure your page/control (XAML file).

- In the namespace section:

    ```xml
    xmlns:u="using:Umbrella.View.Controls"
    ```

- In the XAML:

    ```xml
    u:ListViewBaseCommand.Command="{Binding [ShowNewPage]}"
    ```

- If a parameter is needed:
    ```xml
    u:ListViewBaseCommand.CommandParameter="Samples"
    ```



## Known issues

- On Android and iOS, when specifying the `CommandParameter`, the ViewModel gets the `SelectedItem` as a command parameter and not the `String` value specified.