# PartitionConverter

## Summary

`PartitionConverter` is a UI converter that is intended to partition the entire double set in xaml.
It allows set a list of `Partition` directly in its content.
Those partitions are helpful to : 
 * Match a discrete value. See `DiscretePartition`.
 * match a certain range of values. See `IntervalPartition`

 The converter also allows to set a `PartitionStrategy` when custom computation must be done over the bound value. The `ParityPartitionStrategy` is a good example.
 
## Platform support

| Feature                | UWA | Android | iOS |
| -----------------------|:---:|:-------:|:---:|
| Convert a double value |  X  |    X    |  X  |

## Usage

Use this converter when you have a business rule that needs to split the double set (_-Inf_ to _Inf_) to different values.

## Remarks
 
If many partitions are considered in range, the first matching partition set will convert the value.
For obvious performance reasons, no validation is made with the provided partition sets.
A default value can be provided, in which case it will act as a partition itself, matching any other element of the set.
If both `Partitions` and a `PartitionStrategy` are provided, the `Partitions` have precedence.

## Extensibility

The `PartitionConverter` allows extensibility. 2 abstract classes are available for this.

### Partition extensibility class

If necessary, you can implement the `Partition` abstract class. Two implementations are already provided :

#### DiscretePartition
The `DiscretePartition` class is an implementation of the `Partition` class.
It allows to match an exact value in the numeric set of doubles.
It allows to pass an accuracy parameter (See : _DoubleEqualityAccuracy_). This accuracy can be 

* _Minimal_ : 0.1
* _Weak_    : 0.001
* _Normal_  : 0.000001
* _Precise_ : 0.000000001
* _Maximal_ : 0.000000000001

The default accuracy is set to normal, which should normally be correct for a daily usage.

It is recommended to use the `DefaultValue` property of the `PartitionConverter` when using this `Partition` implementation.

##### Example

```XML
<uc:PartitionConverter x:Key="MyPartitionConverter"
                       DefaultValue="Value is not 5">
    <uc:DiscretePartition Value="5"
                          InRangeValue="Value is exactly 5" />
</uc:PartitionConverter>
```

#### IntervalPartition
The `IntervalPartition` class is an implementation of the `Partition` class.
It allows to determine whether a conversion value is contained in a certain range.
It allows to set the `BoundMode` to `Inclusive` or `Exclusive`, depending on the situation.
It is recommended to use the `DefaultValue` property of the `PartitionConverter` when using this `Partition` implementation.

##### Example
```XML
<uc:PartitionConverter x:Key="MyPartitionConverter"
                       DefaultValue="Value is less than or equal to 0 OR strictly more than five">
    <uc:IntervalPartition LowerBound="0"
                          LowerBoundMode="Inclusive"
                          UpperBound="5"
                          UpperBoundMode="Exclusive"
                          InRangeValue="Value is strictly more than 0 AND less than or equal to five" />
</uc:PartitionConverter>
```

### PartitionStrategy extensibility class

If necessary, you can implement the `PartitionStrategy` abstract class. One implementation is already provided :

#### ParityPartitionStrategy
The `ParityPartitionStrategy` class is an implementation of the `PartitionStrategy` class.
It allows to convert a conversion value depending on the parity of a double.

##### Example
```XML
<uc:PartitionConverter x:Key="ParityToString">
    <uc:PartitionConverter.PartitionStrategy>
        <uc:ParityPartitionStrategy OddValue="Value is odd"
                                    EvenValue="Value is even" />
    </uc:PartitionConverter.PartitionStrategy>
</uc:PartitionConverter>
```

### Scenarios
* _Need to show a button when there are at least three items in a list_
``` XML
<uc:PartitionConverter x:Key="ItemsLengthToVisibility"
                       DefaultValue="Collapsed">
    <uc:IntervalPartition LowerBound="3"
                          InRangeValue="Visible">
</uc:PartitionConverter>

<Button Visibility={Binding [Items].Length, Converter={StaticResource ItemsLengthToVisibility}} />
```

* _Need to show a button when there are exactly three items in a list_
``` XML
<uc:PartitionConverter x:Key="ItemsLengthToVisibility"
                       DefaultValue="Collapsed">
    <uc:DiscretePartition Value="3"
                          InRangeValue="Visible">
</uc:PartitionConverter>

<Button Visibility={Binding [Items].Length, Converter={StaticResource ItemsLengthToVisibility}} />
```

* Need to display a different background depending on the parity of a number
``` XML
<uc:PartitionConverter x:Key="ParityToBackgroundColor">
    <uc:PartitionConverter.PartitionStrategy>
        <ParityPartitionStrategy EvenValue="{StaticResource BlueBackgroundColor}"
                                 OddValue="{StaticResource YellowBackgroundColor}" />
    </uc:PartitionConverter.PartitionStrategy>
</uc:PartitionConverter>

<Button Background={Binding [Items].Index, Converter={StaticResource ParityToBackgroundColor}} />
```

* More complex scenarios are possible. Imagine a fake scenario like this one.

| Interval    | Converted Value     |
| ----------- |:-------------------:|
| _]-inf, 0[_ |  _negative_         |
| _[0, 4]_    |  _0 to 4 inclusive_ |
| _]4, 5[_    | _default_           |
| _[5, 5]_    | _five_              |
| _]5, +inf[_ | _more than five_    |

This converter allow to do the exact scenario found above :

``` XML
<uc:PartitionConverter x:Key="ImaginaryConverter"
                       DefaultValue="default">
    <uc:IntervalPartition UpperBound="0"
                          UpperBoundMode="Exclusive"
                          InRangeValue="negative" />
    <uc:IntervalPartition LowerBound="0"
                          UpperBound="4"
                          InRangeValue="0 to 4 inclusive" />
    <uc:DiscretePartition Value="5"
                          InRangeValue="five" />
    <uc:IntervalConverter LowerBound="5"
                          LowerBoundModel="Exclusive"
                          InRangeValue="more than five" />
<uc:PartitionConverter />
```

## Known issues

Binding `MinValue` and `MaxValue` of numeric types into this converter fails to convert the value properly.
