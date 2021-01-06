#FromObjectToFormattedStringConverter

## Summary
A converter that applies a String.Format to a given object.​

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
| CultureOverride                             |  x  |    x    |  x  |

## Usage
This converter may be used when we need to wrap the string representation of an object in a context.  (E.G. currencies.​)

## Parameters
The parameter should be set with the proper string format.​
CultureOverride can be used to override the device's default UI culture

## Default behavior
If no parameter is set, this converter will simply return the string representation of the value.

## Known issues

None.
