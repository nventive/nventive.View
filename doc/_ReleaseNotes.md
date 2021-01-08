# Release notes 

## Next Version 
### Features
* Added new properties `OpenUriCommand`, `HyperlinkUnderlineStyle` and `HyperlinkDefaultFontWeight` in `HtmlTextBlockBehavior`.
* Add support for Android 10
* Add support for Android X
* Update xamarin build download to 0.10.0 to add subdependencies bug fix
* 
### Breaking changes 
* updated CommonServiceLocator to 2.0.5
* Dropped support for MonoAndroid80 target
* Updated to Uno.UI 3.0+
* 
### Bugfixes
* Fixed an insert issue with FormattingTextBoxBehavior where if typing right before an filler character will cause the caret position to uncorrectly move to the left by 1.
* 

## 3.9.0
### Features 
* Add possibility to manually control the materialization of the view in `DelayControl`.
### Bugfixes 
* Fix InnerPlay() which could be called by public Play() on iOS before the animation was properly loaded.
* fixed FormattingTextBoxBehavior cursor position issue after formatting
* fixed FormattingTextBoxBehavior flicker on iOS

## 3.8.0
### Features
 * Added CultureOverride to FromObjectToFormattedStringConverter. This allows the consumer of this converter to override the device's UI culture.
 * Added clarifying comment to [FieldValidation.md](FieldValidation.md) for ``AttachValidation(DynamicProperty)`` 
 * PeekingFlipView now centers items when index changed without swiping (previously would only bring items into view -edge)
 ### Breaking changes
 * MultipleSelectionBehavior: Removed the ListSelectionMode attached property. Set the SelectionMode of the ListView instead.
### Bug fixes
 * Fix HyperlinkExtension.Command to support CanExecute properly.

## 3.7.0
### Features
 * Updated support libraries to 28.0.0.1 for Android 9
 * Add [FieldValidation](FieldValidation.md) Recipe. It's in its own nuget package (Umbrella.View.Validation).
 * Add [ParallaxListView](ParallaxListView.md) Control. It allows for a ListView Header with a parallax effect on scroll.
 * Add [ListViewBaseMultiColumnExtension](ListViewBaseMultiColumnExtension.md). The extension helps ListView to have multiple columns and adjusts each item size based on screen size.
 * Add [ProportionalPanel](ProportionalPanel.md). A custom panel which maintains proportion on the child controls.
 * UITesting is now available for Umbrella.View
 * Fixed InputPane sample to fit with iPhone X (VisibleBoundsPadding)
 * Fixed InputPaneExtensions.PanIntoView formula to work with iPhone X

### Bug fixes
 * Fixes potential `NullReferenceException` when a materialization is requested but the control has not ben templated yet.
 * Fixes `HtmlTextBlockBehavior` to ignore carriage return, line feed and tab characters, as well as to recognize both forms of <br/> tag.

## 3.6.0
### Features
 * Updated Umbrella.View to target MonoAndroid90
 * Updated Umbrella.View to target uap 17763
 * Added support for [SourceLink](https://github.com/dotnet/sourcelink)

## 3.5.0
### Features
 * Adds `MultipleTapExtension` which allows you to trigger a command after multiple taps. Useful for hidden features like diagnostics.
 * 140123 - Added `PlaceholderContentPresenter` for the `WatermarkedDatePicker` in order to ease the styling + updated documentation.

### Breaking changes
 * `Expander` default template now uses `HorizontalContentAlignment` to align the header and the items.
 * `PlaceholderText` (string) was replaced by `Placeholder` (object) for the `WatermarkedDatePicker`.

### Bug fixes
 * [iOS][SwipableItem] Swipable items are not closed when user interacts with
 * Fixes bug where the PrettyDateFormatterString were not available in English.

## 3.4.0

### Breaking changes
 * `WaitControl` was renamed to `DelayControl` ref:[DelayControl](DelayControl.md)
 * AnimatedControl and AnimatedContentControl now stop their animation when unloaded. This behaviour can be disabled by setting the `DisableOnUnload` property to `false`.

### Bug fixes
 * AnimatedControl and AnimatedContentControl now stop their animation when unloaded.

## 3.3.0

### Features
 * Added iOS destructive style support for `MessageDialogService` commands

## 3.2.0

### Features

 * Added ControlExtensions.GotFocusBehavior to allow the execution of a Command when a Control acquires focus.
 * Added ControlExtensions.LostFocusBehavior to allow the execution of a Command when a Control loses focus.
 * Added IsActive property to Lottie animation to allow binding of the Play() & Stop() actions
 * Added ScrollViewerExtensions.ScrollIndicatorStyle to allow customizing of scroll bar style on iOS
 * Added HyperlinkExtensions to allow for inline Commands
 * Added [WaitControl](DelayControl.md) to delay loading some content, so that a page can offer overall a more fluid loading experience
 
### Breaking changes

 * `TextBoxEnterCommand` removed. If using  `TextBoxEnterCommand` use `TextBoxBehavior.EnterCommand` instead
 * Moved ImageEx to Umbrella.View.Legacy, use ImagePresenter instead.
 * Removed VisibleBoundsPadding from Umbrella.View, you should use the one from Uno.UI.Toolkit.
 * Renamed ControlAutoFocusBehavior to ControlExtensions.AutoFocus, use of this now needs to call on ControlExtension instead
 * MessageDialogBuilder: Trimmed down possible DialogResults values to OK, Accept, Cancel and No, also removing many helper methods for obsolete DialogResults. Accept and No button labels should be customized with a different resource key for each different dialog, while OK and Cancel should simply have one (localized) label for the entire app. Also, the default value for the dialog response is no longer None, but rather Cancel. Buttons are now displayed in the order in which they are added, instead of the opposite (therefore, first add the cancel button then the accept button).
 * Changed `ListViewBringIntoViewSelectedItemBehavior.Alignment` to `ListViewBaseExtensions.BringSelectedItemIntoViewAlignment`. Possible values are now `None`, `ClosestEdge` and `Leading`. If you were using `Default`, `ClosestEdge` is the equivalent.

### Bug fixes

 * 111637 [Android] Fixed an issue where Lottie animations using merged layers would not display properly
 * 102916 Added error when using images in Lottie animations since they are not supported
 * Fixed Samples logging setup
 * 130420 Fixed ListViewBringIntoViewSelectedItemBehavior.Alignment="Default" not working.


## 3.1.0

### Features
 * On ListViewBringIntoViewSelectedItemBehavior, added ability to specify the ScrollAlignment.

### Breaking changes
 * MessageDialogBuilderDelegate moved from UmbrellaApp to Umbrella.View.MesssageDialog.UWP
 * Deleted TextBoxBehavior.Text property, use the Textbox's Text property instead
 * Deleted the TextBoxBehavior.Behavior property
 * Deleted TextBoxBehavior.AutoUpdateBindingDelay, you should use the one from TextBoxExtendedProperties
 * Changed ListViewBringIntoViewSelectedItemBehavior.IsEnabled to ListViewBringIntoViewSelectedItemBehavior.Alignment, you will have to replace it with ListView[...]Behavior.Alignment = Default.
 * Removed the 'Umbrella.View.Panels' namespace, moved all panels to the 'Umbrella.View.Controls' namespace.

### Bug fixes
 * MessageDialogBuilder now uses the Delegates from Uno
 * 128073 [Android] Fixed an issue where textbox bindings were breaking when adding a FormattingTextBoxBehavior
 * 128719 [Android] MembershipCardControl can be seen flipping when closing

## 3.0.0

### Features
 * Initial version based on **Umbrella V2**.
 * MembershipCardControl.cs has four properties changed to Dependency Properties: IsBrightnessSetToMaximum, IsStatusBarHidden, IsKeyboardHidden, IsOtherPopupsClosed.

### Breaking changes
 * `FromNullableBoolToDefaultValueConverter` renamed to `FromNullableBoolToCustomValueConverter`
 * `FromNullableToDefaultValueConverter` renamed to `FromNullableToCustomValueConverter`
 
### Bug fixes
 * 126517 [iOS][TextBoxBehavior] TextBoxBehavior.NextControl scroll into view not working on iOS
 * 126517 [iOS][PasswordBoxBehavior] PasswordBoxBehavior.NextControl scroll into view not working on iOS
 * [iOS] Fix an issue with LandscapeUprightPanel and MembershipCardControl where rotating the device sometimes caused an unhandled exception
 * 126674 [UWP][Android] Fixed ControlAutoFocusBehavior.IsAutoFocus does not set cursor position correctly at the end if textbox has content
 * 128075 [UWP][Android][iOS][ListView] Added multi selection behavior for ListViewBase to be used for SelectedItems property binding
 * 129114 [Android] MembershipCardControl opens when resuming app after minimizing with back button
