# FromEnumToStringConverter

## Summary

`FromEnumToStringConverter` converts an **enum** value to a localizable string using the resource manager.

Properties:

**CharacterCasing** (CharacterCasingOption)  
* None *(default value)*
* LowerCase (applies `string.ToLower()`)
* UpperCase (applies `string.ToUpper()`)

## Platform support

| Feature                           | UWA | Android | iOS |
| --------------------------------- |:---:|:-------:|:---:|
| Localize the enum values          |  X  |    X    |  X  |
| Casing option: Upper Case         |  X  |    X    |  X  |
| Casing option: Lower Case         |  X  |    X    |  X  |
| Casing option: Title Case         | --  | --      | --  |
| Casing option: Sentense Case      | --  | --      | --  |

## Usage

### Resource

In projects that started off from the **MyUmbrellaApp** base, this converter is **injected** in `ResourcesModule.cs`, therefore its resource key `EnumToStringConverter` is generated in code behind.

In projects that don't have the converter, this is how it can be injected:

``` C#
private static void RegisterResourcesCore(Container container, ResourceDictionary resources)
{
    resources.Add("EnumToStringConverter", new FromEnumToStringConverter(container.Resolve<IResourcesService>()));
}
```

The constructor can take an `IResourcesService` or a `ResourceManager`.

### Localized string key format
[*Enum name*]_[*Enum key*]

#### Example
``` C#
public enum FlightStatus
{
    Unknown = 0,
    Scheduled,
    OnTime,
    Delayed
}
```

| Key                               | String (fr) | String (en) |
| --------------------------------- |:-----------:|:-----------:|
| FlightStatus_Unknown              | Inconnu     | Unknown     |
| FlightStatus_Scheduled            | Planifié    | Scheduled   |
| FlightStatus_OnTime               | À l'heure   | On Time     |
| FlightStatus_Delayed              | Retardé     | Delayed     |

``` xml
<TextBlock Text="{Binding Data.FlightStatus, Converter={StaticResource EnumToStringConverter}}" />
```

## Known issues

None.
