# FormattingTextBoxBehavior

## Summary

The `FormattingTextBoxBehavior` allows user input text to be modified while typing to conform to a specified format.

## Platform support

| Feature                                         | UWA | Android | iOS |
| ----------------------------------------------- |:---:|:-------:|:---:|
| Modify text during typing                       |  X  |    X    |  X  |
| Uses TextBox.TextChanging event                 |  X  |    -*   |  -  |

* On Android, the native `IInputFilter` interface is used to provide acceptable performance.

## Usage

````
<TextBox ue:FormattingTextBoxBehavior.TextFormat="0000 0000 0000 0000"
		 ue:FormattingTextBoxBehavior.IsEnabled="True" />
````

## Supported formatting notation

 The specified format follows the following convention:
'#' allows anycharacters
'A' : capital letter
'a' : lower case letter
'0' : digit
'N' : Capital alphanumeric
'n' : lower case alphanumeric

Example of formats:

- (000) 000-000 = Phone number
- 0000 0000 0000 0000  = credit card
- A0A 0A0 = code postal

## Known issues

On some Android devices (older Samsung phones?), the native `IInputFilter` won't apply characters that aren't typeable with the configured
`InputScope` - eg, if `InputScope="Number"` is set, then it won't add spaces. This causes `FormattingTextBoxBehavior` to fall back on
setting the TextBox.Text property, which is less performant and causes characters to be dropped when typing quickly. The workaround is to
use an `InputScope` on the `TextBox` which permits the inserted characters, such as `NumericFullWidth` in this example.
