# DelayControl

## Summary

The `DelayControl` is a control which that delays view creation until a certain time has passed after either loading is complete or
the DataContext is set to a non-null value. Also offers visual states Waiting and Ready for animation purposes (by default, a short fade-in).

It is frequently used in pages with a heavy amount of content in order to load this content progressively, avoiding blank screens.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
| `DelayAfterDataContext`                     |  X  |    X    |  X  |
| `DelayAfterLoad`                            |  X  |    X    |  X  |
| `DelayAfterManualTrigger`                   |  X  |    X    |  X  |
| `IsAllowedToMaterialize`                    |  X  |    X    |  X  |
| `ApplyTemplatePriority`                     |  X  |    X    |  X  |
| `WaitForInnerView`                          |  X  |    X    |  X  |

## Usage

Add the following namespace to your `.xaml` file:

```xml
xmlns:u="using:Umbrella.View.Controls"
```
Add the `DelayControl` to your `.xaml` file:

```xml
<u:DelayControl Height="50"
               Width="50">
	<DataTemplate>
		<!-- your content -->
	</DataTemplate>
</u:DelayControl>
```

The default style can be found here: [DelayControl.xaml](../src/Umbrella.View/Controls/DelayControl/DelayControl.xaml)

### Properties

 * `DelayAfterDataContext`: After the DataContext receives a non-null value, how long to wait until loading the content. A value of 0 is ignored.
 * `DelayAfterLoad`: After the DelayControl itself is loaded, how long to wait until loading the content. A value of 0 is ignored.
 * `DelayAfterManualTrigger`: After the `IsAllowedToMaterialize` is set to true, how long to wait until loading the content. A value of 0 is ignored.
 * `IsAllowedToMaterialize`: This is `true` by default, but you can also control this value to manually control the materialization of the view.
   * If you control this with a binding, don't forget to set the `FallbackValue` to **`false`**.
	 ```xml
	 <u:DelayControl IsAllowedToMaterialize="{Binding [IsReadyToLoad], FallbackValue=False}" />
	 ```
 * `ApplyTemplatePriority`: Which scheduler priority to use to schedule loading the content. For instance, a priority of Idle could serve as a delay by itself.
 * `WaitForInnerView`: When true, the visual state will be changed only once the DelayControl has a reference on the materialized view from its ContentTemplate.

### Visual states

| VisualState name | VisualStateGroup name | Description                                                                                              |
|------------------|-----------------------|----------------------------------------------------------------------------------------------------------|
| `Waiting`        | `WaitingStates`       | The content hasn't been loaded yet                                                                       |
| `Ready`          | `WaitingStates`       | The content has been loaded                                                                              |

### Parts

| Name                      | Type                | Description                                                                                                                      |
|---------------------------|---------------------|----------------------------------------------------------------------------------------------------------------------------------|
| `PART_ContentPresenter`   | `ContentPresenter`  | Used to display the content.                                                                                                     |


## Known issues
