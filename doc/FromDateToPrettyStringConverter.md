# FromDateToPrettyStringConverter

## Summary
This converter is used to provide an easily readable and comprehensible string based on a TimeSpan, DateTime or DateTimeOffset.
## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter is used to convert a TimeSpan, DateTime or DateTimeOffset to a string, E.G. to display when a file was uploaded.​

## Configuration
| Property name | Type | Description |
| --- | --- | --- |
|NowGetter | Func<DateTime> | Provides the converter with a specific way to fetch the current time. |
| ​Formatter | ​IPrettyDateFormatter | ​Provides the converter with a specific way to format the date. |
| ​Mode | ​PrettyDateMode | ​Changes the string so that it formats the string in the past or present.  (E.G. 14 minutes vs. 14 minutes ago) |

## Default behavior
NowGetter and Formatter have default implementations which provide expectable and standardized results.
Mode is set to Past by default.
If the value is null, an empty string will be returned

## Known issues

None.
