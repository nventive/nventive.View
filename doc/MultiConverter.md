# MultiConverter

## Summary
A converter that chains multiple converters in order to provide a result.  Each provided will be executed in the order they are provided.​​​

## Platform support

| Feature                                     | UWA | Android | iOS |
| ------------------------------------------- |:---:|:-------:|:---:|
|                                             |     |         |     |

## Usage
​​This converter may be used when we need multiple steps of conversion from the initial value to the result.​​

## Configuration
| Property name | Type | Description |
| --- | --- | --- |
| Converters | ​List<IValueConverter> | ​The list of all converters to chain.|

## Default behavior
If no converters are provided, MultiConverter should return the initial value.​


## Known issues

None.