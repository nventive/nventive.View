# ImagePresenter

## Summary

The `ImagePresenter` is a control that displays a placeholder until the image source (if any) is successfully opened.
If the image is failing and you want to display a different image than the placeholder, use the ErrorImageSource property.

It is frequently used to display initials and an optional image inside a circle.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
| `ImageSource`                               |  X  |    X    |  X  |
| `ErrorImageSource`                          |  X  |    X    |  X  |
| `PlaceholderContent`                        |  X  |    X    |  X  |
| `PlaceholderContentTemplate`                |  X  |    X    |  X  |
| `PlaceholderContentTemplateSelector`        |  X  |    X    |  X  |
| `CornerRadius`                              |  X  |    X    |  X  |
| `IsCircle`                                  |  X  |    X    |  X  |
| `Placeholder` visual state                  |  X  |    X    |  X  |
| `Image` visual state                        |  X  |    X    |  X  |

## Usage

Add the following namespace to your `.xaml` file:

```xml
xmlns:u="using:Umbrella.View.Controls"
```
Add the `ImagePresenter` to your `.xaml` file:

```xml
<u:ImagePresenter ImageSource="{Binding ImageSource}"
                  PlaceholderContent="{Binding Initials, FallbackValue=' ', TargetNullValue=' '}" />
```

Or for example

```xml
<u:ImagePresenter ImageSource="{Binding ImageSource}"
                  ErrorImageSource="ms-appx:///Assets/ErrorDefaultImage.png"/>
```

The default style can be found here: [ImagePresenter.xaml](../../../Universal/Controls/ImagePresenter/ImagePresenter.xaml)

Note that providing a `FallbackValue` and `TargetNullValue` may be required to avoid having
the `DataContext` being temporarily displayed as text (in place of the `Initials` in the example above).

### Properties

 * `ImageSource`: The source for the image.
 * `ErrorImageSource`: The source for the error image.
 * `CornerRadius`: The radius for the corners of the image.
 * `IsCircle`: The value indicating whether `CornerRadius` should be automatically updated to make a circle.
 * `PlaceholderContent`: The placeholder to display when the image isn't opened.
 * `PlaceholderContentTemplate`: The data template that is used to display the placeholder.
 * `PlaceholderContentTemplateSelector`: The data template selector that is used to display the placeholder.

### Visual states

| VisualState name | VisualStateGroup name | Description                                                                                              |
|------------------|-----------------------|----------------------------------------------------------------------------------------------------------|
| `Placeholder`    | `ImageStates`         | The image source is null (`ErrorImageSource` is not set in this case) or hasn't been opened yet          |
| `Image`          | `ImageStates`         | The image source has been opened or the image has failed (`ErrorImageSource` need to be set in this case)|

### Parts

| Name                 | Type           | Description                                                                                                                      |
|----------------------|----------------|----------------------------------------------------------------------------------------------------------------------------------|
| `PART_Grid`          | `Grid`         | Used to calculate the `CornerRadius` when `IsCircle` is true.                                                                    |
| `PART_ImageBorder`   | `Border`       | Used to host `PART_ImageBrush` as its `Background`.                                                                              |
| `PART_ImageBrush`    | `ImageBrush`   | Used to set `ImageSource` and determine `ImageStates` based on its `ImageOpened` event.                                          |
|                      |                | Also used to set `ErrorImageSource` based on its `ImageFailed` event.                                                            |
| `PART_Image`         | `Image`        | Can be optionally used in place of Border+ImageBrush, in cases where performance is important and rounded corners aren't needed. |

### Shapes

| Shape     | Width | Height | CornerRadius   | IsCircle        |
| ----------|-------|--------|----------------|-----------------|
| Rectangle | Any   | Any    | None (default) | False (default) |
| Square    | Fixed | Fixed  | None (default) | False (default) |
| Ellipse   | Fixed | Fixed  | Any            | False (default) |
| Circle    | Any   | Any    | None (default) | True            |

## Known issues

 * `PART_ImageBrush` must be set as the `Background` of `PART_ImageBorder` to workaround [an issue with `GetTemplateChild()` in Uno](https://nventive.visualstudio.com/Umbrella/_workitems/edit/65602).
