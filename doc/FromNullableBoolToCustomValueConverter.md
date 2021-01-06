# FromNullableBoolToDefaultValueConverter
This converter is used to get a default value if a nullable boolean is true or otherwise.

## Summary

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter may be used to select betweem two values based on a boolean value.​

## Configuration
| Property name | Type | Description |
| --- | --- | --- |
| ​NullOrFalseValue | object | The default value to return if the value is null or false. |
| ​TrueValue | ​object | ​The default value to return if the value is tru . |

## Default behavior
NullOrFalseValue and TrueValue need to be different in order for reverse conversion to work.

## Known issues

None.