# ToggleSwitchBehavior

## Summary

The `ToggleSwitchBehavior` allows ToggleSwitch to execute and ActiveCommand when the user toggles the swicth, 
but not when the switch is toggled by data binding.

## Platform support

| Feature                                                                                         | UWA | Android | iOS |
| ----------------------------------------------------------------------------------------------- |:---:|:-------:|:---:|
| Execute an active command only when the user toggles the switch                                  |  X  |    X    |  X  |

## Usage

### Configure your toggle switch (XAML file).
- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:

    ```xml
		
		<ToggleSwitch
			ue:ToggleSwitchBehavior.IsOn="{Binding Data.IsEmailNotificationsEnabled}"
			ue:ToggleSwitchBehavior.ToggleActiveCommand="{Binding Parent[EnableEnableNotifications]}"/>
    ```
	
Warning: 
- You cannot bind the `ToggleSwitch.IsOn`, your binding will be erased.
- You cannot bind the `ToggleSwitchBehavior.IsOn` in two way mode, the property is not updated when the switch gets toggled. 
  You have to use the `ToggleActiveCommand` instead and update the entity (i.e. `Data.IsEmailNotificationsEnabled`).
  (For more information, search for the _Calculated property_ concept.)

## Known issues
None.
