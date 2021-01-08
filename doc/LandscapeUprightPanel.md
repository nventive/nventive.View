# LandscapeUprightPanel

## Summary

The `LandscapeUprightPanel` is a container made for apps which are locked into a single portrait orientation and need to display content when the device is rotated into landscape.

Since the device does not automatically adjust itself for this orientation change, the `LandscapeUprightPanel` adjusts its content manually.

That way, when the device is in landcape, the content is shown upright, even if the device is upside-down.

When the device is in portrait orientations, the content is displayed as rotated and keeps the same dimensions.

This is particularly useful when showing information for the purposes of scanning it, such as member cards. See the [MembershipCardControl](MembershipCardControl.md)

## Platform support

| Feature                                         | UWA | Android | iOS |
| ----------------------------------------------- |:---:|:-------:|:---:|
| Display content upright in landscape            |  -  |    X    |  X  |


## Usage

Add a reference to LandscapeUprightPanel in your XAML. Make sure that its dimensions stretch it to occupy the full screen and do the same with its child.

* LandscapeUprightPanel must contain only one direct child.
* The application must be locked to portrait:

```
DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
```

* The Panel must stretch to fill the screen
* Any RenderTransform on the child will be overridden
* Leaves a 1-2px gap around the children on iOS.
* Doesn't let touches go through panel (default transparent background).


## Known issues

On iOS, if the direct child of the LandscapeUprightPanel has a BorderThickness, that border is displayed incorrectly. To work around this, add a simple Grid as the direct child of the `LandscapeUprightPanel` and place your content inside that Grid.