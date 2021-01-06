# ListViewBaseMultipleSelectionBehavior

## Summary

The `ListViewBaseMultipleSelectionBehavior` is a class that exposes attachable property `SelectedItems` that binds the SelectedItems of a ListViewBase control to it.
This permits SelectedItems to be bound Two-Way instead of the One-Way binding that is otherwise allowed.

## Platform support

| Feature                     | UWA | Android | iOS |
| --------------------------- |:---:|:-------:|:---:|
| SelectedItems               |  X  |    X    |  X  |

## Usage

On a ListViewBase control (ListView, GridView and etc), make sure that SelectionMode is set to Multiple and add the SelectedItems attached property to the Xaml file.

### Configure your page/control (XAML file).

- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```

- In the XAML:

    ```xml
    SelectionMode="Multiple"
    ue:ListViewBaseMultipleSelectionBehavior.SelectedItems="{Binding [SelectedItems], Mode=TwoWay}"
    ```


## Known issues
- The ListView.SelectedItems returns duplicates when using **enums**. The workaround for this is to *use the **string values** of the enum instead of the enum itself, and use **casting** as needed* (See sample).
