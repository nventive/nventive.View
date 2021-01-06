# TextBoxExtendedProperties

## Summary

Properties which can be added to modify the way the TextBox behaves.

## Platform support

| Feature                           | UWA | Android | iOS |
| --------------------------------- |:---:|:-------:|:---:|
| Throttling                        |  X  |    X    |  X  |

## Usage

To enable Throttling, set the AutoUpdateBindingDelay property to the desired TimeSpan. As long as the Text has been modified for a short time span than this value, the Binding will not be updated.

1. In your `.xaml` file, add the following namespace.

```xml
xmlns:ue="using:Umbrella.View.Extensions"
```

2. In your `.xaml` file, enable the behavior on the textbox.
```xml
<TextBox Text="{Binding [BoundText], Mode=TwoWay}"
         ue:TextBoxExtendedProperties.AutoUpdateBindingDelay="0:0:1"/>
```

As long as the user types characters at less than 1 second of interval, BoundText will not be updated.

## Known issues

None.