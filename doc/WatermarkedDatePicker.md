# WatermarkedDatePicker

## Summary

`WatermarkedDatePicker` uses and applies the Native DatePicker for each platform.
It's an extended DatePicker allowing the use of a PlaceholderContentPresenter.

## Platform support

| Feature                        | UWA | Android | iOS |
| -------------------------------|:---:|:-------:|:---:|
| PlaceholderVisibleStateName    |  X  |    X    |  X  |
| PlaceholderInvisibleStateName  |  X  |    X    |  X  |
| PlaceholderStatesGroupName     |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:u="using:Umbrella.View.Controls"
    ```
- In the XAML:
    ```xml
		
		<u:WatermarkedDatePicker Style="{StaticResource YourCustomWatermarkedDatePickerStyle}"/>

    ```
- For Style see, file: Uno.Shared\Controls\WatermarkedDatePickerPage.xaml.
  You can customize the DatePicker in various ways to suit your app.
	
## Properties
'Header': Gets or sets the content for the control's header.
'HeaderTemplate' : Gets or sets the DataTemplate used to display the content of the control's header.
'PlaceholderDate': Gets or sets the initial Placeholder Date (the date that users see before selecting a date, it is applied only if an intial date is not set(binded)).
'Placeholder': Gets or sets the placeholder content that you can dynamically show or hide depending on DatePicker visual states.
'PlaceholderTemplate' : Gets or sets the DataTemplate used to display the content of the control's placeholder.

## Known issues

None.
