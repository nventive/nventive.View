# FromSizeToPrettyStringConverter

## Summary
This converter is used to provide an easily readable and comprehensible string based on a file size.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter is used to convert a file size (double) to a string.


## Configuration

| Property name | Type | Description |
| --- | --- | --- |
| Formatter​ | IPrettySizeFormatter | Provides the converter with a specific way to format the size. |
| ​NumberFormat | string | Provides the converter with a specific way to format the number.|
| Separator | string | Sets the separator string between the number and the units.|

## Default behavior
Formatter has a default implementations which provide expectable and standardized results.
NumberFormat is set to a two decimal floating point format by default.
Separator is set to a single whitespace by default.
If the value is null, an empty string will be returned.
## Known issues

None.