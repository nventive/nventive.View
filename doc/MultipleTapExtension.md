# MultipleTapExtension

## Summary

`MultipleTapExtension.Command` allows the execution of a `ICommand` once the tap count reaches `MultipleTapExtension.TapCount`. This can be useful for hidden features like diagnostics. This extension can be attached to any `FrameworkElement`.

## Platform support

| Feature                         | UWA | Android | iOS |
| ------------------------------- |:---:|:-------:|:---:|
| Command                         |  X  |    X    |  X  |
| CommandParameter                |  X  |    X    |  X  |
| TapCount                        |  X  |    X    |  X  |

## Usage

- In the namespace section:
```xml
xmlns:ue="using:Umbrella.View.Extensions"
```

- In the XAML:
```xml
<Grid ue:MultipleTapExtension.Command="{Binding [MyCommand]}"
        ue:MultipleTapExtension.TapCount="3">
    <TextBlock Text="Version 1.0.0" />
</Grid>
```

- In the ViewModel:
```csharp
// In the constructor
Build(b => b
    .Properties(pb => pb
        .AttachCommand("MyCommand", cb => cb.Execute(ExecuteMyCommand))
    )
);

// In the class body
private async Task ExecuteMyCommand(CancellationToken ct) { }
```

## Known issues
None.
