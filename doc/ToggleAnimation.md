
#ToggleAnimation

##Summary

The `ToggleAnimation` is a control which plays a Lottie animation when a check box, a toggle button or a toggle switch changes states.
See the [Lottie.md] (doc/Lottie.md) documentation for Lottie specific informations.

##Mode 

###AutoReverse

The `AutoReverse` mode is used if the animation's JSON file contains both the forward and reverse animations.

###ManualReverse

The `ManualReverse` mode is used when the animation's JSON file only contains the forward animation. It allows different animations for each transitions.

##Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
| `LottieToggleButton`                        |  X  |    X    |  X  |
| `LottieToggleSwitch`                        |  -  |    -    |  X  |
| `LottieCheckBox`                            |  X  |    X    |  X  |
| `Mode`                                      |  X  |    X    |  X  |

## Usage

1. Add a reference to Umbrella.View.Lottie to iOS, Android and UWP project heads

2. Import JSON animation file into project

    > Build action for animation file must be 
    > * AndroidAsset on Android
    > * BundleResource on iOS
    > * Content on UWP

    For Android, you may need to store your .json and .png files in another nested "Assets" folder, like Assets/Assets/Animations.

3. Add the required namespace into the XAML file

	```
	xmlns:lottie="using:Umbrella.View.Lottie"
	```
	
1. To display animation, simply use

    ```
   <lottie:ToggleAnimation FileName="Assets/Animations/mockAnimation.json"
						   x:Name="ToggleAnimation"
						   Mode="AutoReverse"
						   IsChecked="True" />
    ```

## Known issues
* The lottie `ToggleSwitch` does not work on UWP or on Android
* The lottie `ToggleButton` and lottie `ToggleSwitch` do not appear on UWP because the Generic.xaml file does not work

