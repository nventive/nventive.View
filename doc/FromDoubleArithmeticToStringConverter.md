# FromDoubleArithmeticToStringConverter

## Summary
This converter is used to format a double in a string after applying an arithmetic operation to it.

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
This converter may be used when the numeric data that is obtained is not directly significant to the user, E.G. index 0 is the 1st item of a list.

## Configuration
| Property name | Type | Description |
| --- | --- | --- |
| ​MultiplyBy | double | A value by which the given value should be multiplied. |
| ​DivideBy | ​double | ​A value by which the given value should be divided. |
| ​Add | ​double | ​A value that should be added to the given value. |
| ​Substract | ​double | ​A value that should be subtracted to the given value. |
| ​Modulo | ​double | ​A value that should be used to apply a modulo to the given value. |
| ​FormatString​​ | ​string ​The Double.ToString format that should be used.​|

## Default behavior
For available formats, please refer to : http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx

The precedence of operators is : (((value * MultiplyBy) / DivideBy) + Add - Substract) % Modulo

By default, all values have been set to neutral values so that if no values are set, the result will be the same as the initial value.
By default, the FormatString is a direct string representation of the double value.
## Known issues

None.









