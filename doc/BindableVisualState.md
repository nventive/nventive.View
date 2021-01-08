# BindableVisualState

## Summary

The `BindableVisualState` is a class that exposes attachable properties named `VisualStateName` that lets you bind a string on a `Control` to set a `VisualState`.
This allows you to control the visual states of your view from a ViewModel or anything that can be bound from.

### Details

This behavior is very useful when used on `Page` or `UserControl`. You can create cool animations between states of you pages.
You'll notice that there are four (4) properties with the same name. This allows you to have multiple binding sources for the same control.
It's particularly convenient when having multiple `VisualStateGroup` on the same `Control`.

## Platform support

| Feature                                               | UWA | Android | iOS |
| ----------------------------------------------------- |:---:|:-------:|:---:|
| BindableVisualState.VisualStateName					|  X  |    X    |  X  |
| BindableVisualState.VisualStateName2					|  X  |    X    |  X  |
| BindableVisualState.VisualStateName3					|  X  |    X    |  X  |
| BindableVisualState.VisualStateName4					|  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).

- In the namespace section:

    ```xml
    xmlns:ue="using:Umbrella.View.Extensions"
    ```

- In the XAML:

    ```xml
    ue:BindableVisualState.VisualStateName="{Binding [PrimaryLanguageState]}"
	ue:BindableVisualState.VisualStateName2="{Binding [SavingVisualState]}"
    ```
	Full example [here](https://nventive.visualstudio.com/RxTx/_git/RxTx?path=%2FRxTx.Views%2FContent%2FContentOptions.xaml&version=GBmaster&_a=contents)
### Use your ViewModel to control the visual states

You can use `IDynamicProperty<string>` or `Feed<string>` to expose the VisualStateName.
Use the one that's more appropriate based on your situation.
```csharp
public ContentOptionsViewModel()
{
	Build(b => b
		.Properties(pb => pb
			// Feed<string>
			.Attach(PrimaryLanguageState)

			// IDynamicProperty<string>
			.Attach(SavingVisualState, () => "NotSaving")
		)
	);
}

// (...)

private Feed<string> PrimaryLanguageState => LanguagePreference.Get.Select(cl => cl == LanguagePreferences.Both ? "Visible" : "Collapsed");

private IDynamicProperty<string> SavingVisualState => this.GetProperty<string>();
```

## Known issues

**When using Uno.UI, using this behavior on `UserControl` doesn't work directly.**
If you want to achieve the same result, you need another class the simply inherits from `UserControl` and use that in your xaml instead.

Example:
```xml
<controls:AttachableUserControl x:Class="RxTx.Views.Content.ContentOptions"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	xmlns:ue="using:Umbrella.View.Extensions"
	xmlns:controls="using:RxTx.Views.Controls"
	mc:Ignorable="d"
	ue:BindableVisualState.VisualStateName="{Binding [PrimaryLanguageState]}"
	ue:BindableVisualState.VisualStateName2="{Binding [SavingVisualState]}">
```
```csharp
namespace RxTx.Views.Controls
{
	/// <summary>
	/// This is a workaround the fact that using attached properties on UserControl doesn't work with Uno.UI
	/// http://feedback.nventive.com/topics/257-usercontrol-doesnt-support-attached-properties/
	/// </summary>
	public partial class AttachableUserControl : UserControl
	{
	}
}
```
