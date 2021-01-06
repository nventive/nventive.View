# MembershipCardControl

## Summary

The `MembershipCardControl` is a control to display a membership card (as a popup) when the device is rotated to landscape, in an app which is otherwise locked to portrait. It uses the [LandscapeUprightPanel](../UprightPanel/LandscapeUprightPanel.md) for this.

## Platform support

| Feature                                         | UWA | Android | iOS |
| ----------------------------------------------- |:---:|:-------:|:---:|
| Display popup when device turns to landscape    |  -  |    X    |  X  |
| Display content upright when in landscape       |  -  |    X    |  X  |
| Display popup manually (IsOpen = true)          |  X  |    X    |  X  |
| Set brightness to max when popup opens          |  X  |    X    |  X  |
| Hide keyboard when popup opens                  |  X  |    X    |  X  |
| Close other popups when this popup opens        |  X  |    X    |  X  |
| Close popup on BackButton                       |  -  |    X    |  -  |
| Disable auto popup (IsAutoOpenOnLandscape)      |  -  |    X    |  X  |
| MultiWindow/Splitview support                   |  -  |    -    |  -  |



## Usage

The application must be locked to portrait:

```
DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
```

Add a reference to MembershipCardControl in your XAML. Make sure that its dimensions are streched, same for its child.

Make sure the InitializeDependencies static method is called before the control is loaded - if not, an exception will be thrown.

The default style includes LandscapeUprightPanel as well as an animation to slide it in.


## Known issues

On iOS, if the device is already in landscape at launch, the control will believe the device is in portrait mode and render incorrectly. This is resolved by rotating the device 90 degrees.
https://feedback.nventive.com/communities/1/topics/2637-ios-simpleorientationsensor-returns-wrong-value-at-launch
Using VisibleBoundsPadding inside the MembershipCardControl template is inadvisable, since it will cause performance issues. We recommend using it on the container which contains MembershipCardControl.