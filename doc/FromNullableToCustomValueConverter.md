# FromNullableToCustomValueConverter

## Summary
A converter that is used to return a custom value based on the presence or absence of value.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter may be used to display a custom value whenever the data is not ready.​


## Configuration
| Property name | Type | Description |
| --- | --- | --- |
| ​ValueIfNull | object | The custom value that is returned if the value is null. | 
| ​ValueIfNotNull | ​object | ​The custom value that is returned if the value is not null.​ |

## Default behavior
If ValueIfNotNull is not set, the converter will return the value if it is not null.
If ValueIfNull is not set, the converter will return the custom value of the type if the value is null.

## Known issues

None.
