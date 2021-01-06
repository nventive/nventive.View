#FromEmptyStringToVisibilityConverter

## Summary
A converter that will return a Visibility based on the presence or absence of characters in a string.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter may be used when a string not being specified should be collapsing a TextBlock altogether or a similar circumstance.

## Configuration
| Property name | Type | Description |
| --- | --- | --- |
| Mode | StringValueToVisibilityConverterMode | The mode in which this converter will operate. |

## Default behavior
By default, the Mode is set so that an empty string will result in a collapsed visibility.

## Known issues

None.
