# SplitViewBehavior

## Summary

The `SplitViewBehavior` is a behavior that lets a xaml control interact with a `SplitView`.

This behavior is useful when you want to automatically close the `SplitView` when the user selects a menu item.

The control attached with the behavior must satisfy the following.
- It must be a **child of the SplitView**.
- It must **derive from ButtonBase** or be an **item of a ListViewBase**.

## Platform support

| Feature             | UWA | Android | iOS |
| --------------------|:---:|:-------:|:---:|
| Open the SplitView  |  X  |    X    |  X  |
| Close the SplitView |  X  |    X    |  X  |

## Usage

1. In your `.xaml` file, add the following namespace.

```xml
xmlns:ue="using:Umbrella.View.Extensions"
```

2. In your `.xaml` file, enable the behavior on the appropriate controls.
```xml
<SplitView>
    <SplitView.Pane>
        <StackPanel>
            <Button Content="Open the SplitView"
                    ue:SplitViewBehavior.OpenOnClick="True" />

            <Button Content="Close the SplitView"
                    ue:SplitViewBehavior.CloseOnClick="True" />
        </StackPanel>
    </SplitView.Pane>
</SplitView>
```

## Known issues

None.
