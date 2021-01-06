# LogCounterControl

## Summary

The `LogCounterControl` displays 2 counters overlays; one for warnings and another for errors.

The counters are incremented everytime a warning or error is logged using `Microsoft.Extensions.Logging`.

## Platform support

| Feature                  | UWA | Android | iOS |
| -------------------------|:---:|:-------:|:---:|
| Count warnings           |  X  |    -    |  -  |
| Count errors             |  X  |    -    |  -  |

## Usage

Simply add a `LogCounterControlProvider` as a `ILoggerProvider`.

```csharp
var loggerFactory = container.Resolve<ILoggerFactory>();

var loggerProvider = new LogCounterControlProvider();

loggerFactory.AddProvider(loggerProvider);
```

## Known issues

None.