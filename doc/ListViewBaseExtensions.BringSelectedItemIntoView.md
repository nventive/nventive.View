# ListViewBringIntoViewSelectedItemBehavior

## Summary

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
| Bring into view the selected item           |  x  |    x    |  x  |
| Specify a Leading alignment                 |  x  |    x    |  x  |
| Specify a ClosestEdge alignment             |  x  |    x    |  x  |

## Usage
- In the namespace section:

```xml
xmlns:ue="using:Umbrella.View.Extensions"
```
- In the XAML:
```xml
<ListView ItemsSource="{Binding SampleItems}"
          SelectionMode="Single"
          ue:ListViewBaseExtensions.BringSelectedItemIntoViewAlignment="ClosestEdge"/> <!-- Or "Leading" -->
```

