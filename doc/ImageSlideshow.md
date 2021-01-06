# ImageSlideshow control

## Summary

## Why?

This control is really useful to display multiple images or items in a *Swipable FlipView* or an *Animated Image Carousel*.
We use it in several projects; this is why we decided to include this control in Umbrella.

## Platform support

## Usage

## How
`
xmlns:u="using:Umbrella.View.Controls"
`
`
<u:ImageSlideshow ItemsSource="{Binding [SourceItems]}"/>
`

## Properties

- **ItemTemplate** - Create you own item template

- **SelectedIndex** - If you want to start the ImageSlideshow to another index than 0

- **IndexIndicatorTemplate**- Can be whatever you want, an ellipse, a square

- **SelectedIndexIndicatorTemplate** - The selected indicator template, can be whatever you want

- **AutoRotate** - Bool - if you want the ImageSlideshow to automatically move to the next item after a period of time.

- **AutoRotateRewindEnabled** - Bool - if using the AutoRotate function, when the ImageSlideshow reach the last item, there is a 'rewind' animation.

- **DisplayTime** - How long each item is displayed. Default value: 2000 Milliseconds.

- **RewindTime** - 'rewind' animation time per item. Default value: 80 Milliseconds.

- **ShouldPreserveSelectedIndex** - If you want the ImageSlideshow to keep the same index selected when the ItemsSource is changed/updated. Default value: false.

## Known issues

None.
