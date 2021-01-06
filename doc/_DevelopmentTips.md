# Development tips

## Multitargeting

The projects in this solution are **multitargeted**. 

This means that each project is compiled to generate one assembly per target.

The available targets are:
- Windows (UWP)
- iOS
- Android 8.0
- Android 9.0
- Net461
- WebAssembly (WASM)

Compiling all targets can take some time.

In order to compile only specific targets (e.g. only Android 8.0) and save time, update the [crosstargeting_override.props](../build/crosstargeting_override.props.sample) file.