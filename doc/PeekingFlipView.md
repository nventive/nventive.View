# PeekingFlipView control

## Summary

## Why?

Getting a ListView to behave like a FlipView, using snappoints, in order to have have a FlipView that displays a "peek" of the items on either side of the selected one.

## Platform support

| Feature                                         | UWA | Android | iOS |
| ----------------------------------------------- |:---:|:-------:|:---:|
|                                                 |     |         |     |

## Usage

## How
`
xmlns:u="using:Umbrella.View.Controls"
`
`
<u:PeekingFlipView Height="200"
				   Width="200"
				   ItemsSource="{Binding [SourceItems]}">

`

## Properties

All properties inherited from ListView.

The SelectedIndex property is automatically set based on whichever item is overlaying the center of the PeekingFlipView. In all cases except for items at the beginning and end of the list, this is equivalent to whichever one is currently "snapped". 

## Hacks and FAQs

- If the PeekingFlipView is considerably wider than the item views inside, it may be difficult to get the endpoint items into the middle where they're selected. In cases like this, one may apply Padding to left and right sides of the PeekingFlipView control, ensuring that all enclosed items are equally accessible.

- It seems to be necessary to provide an explicit Width/Height for a PeekingFlipView in iOS and Android. In the iOS sample, the view would not show up at all if they are not provided. In Android, the view tried to take up more space than was available, and threw an IllegalStateException.


## Known issues

None.
