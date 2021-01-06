# HyperlinkExtension

## Summary

`HyperlinkExtensions.Command` allows the execution of a Command when a user taps on a `HyperLink`. 

## Platform support

| Feature                                            | UWA | Android | iOS |
| -------------------------------------------------- |:---:|:-------:|:---:|
| Command triggers when a user taps on a Hyperlink   |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:ue="using:Umbrella.View.Extensions"
    ```
- In the XAML:
    ```xml
	<Hyperlink ue:HyperlinkExtensions.Command="{Binding [HyperlinkClicked]}">Click Here</Hyperlink>
    ```
- In the ViewModel:
    ```csharp
    // In the constructor
    Build(b => b
        .Properties(pb => pb
            .AttachCommand("HyperlinkClicked", cb => cb.Execute(OnHyperlinkClicked))
        )
    );

    // In the class body
    private async Task OnHyperlinkClicked(CancellationToken ct) { }
    ```
	`OnHyperlinkClicked` will be called everytime the Hyperlink is tapped.

### (Optional) Using a CommandParameter
It is possible to provide a `CommandParameter` along with the Command. To do so, we need to:
- Specify the `CommandParameter` in the XAML:
    ```xml
    <Hyperlink ue:HyperlinkExtensions.Command="{Binding [HyperlinkClicked]}"
			   ue:HyperlinkExtensions.CommandParameter="Your parameter">Clickable Text</Hyperlink>
    ```
- In the ViewModel:
    ```csharp
    private async Task<System.Reactive.Unit> OnHyperlinkClicked(object param, CancellationToken ct) 
    {
        // Write logic here...
        return null;
    }
    ```
## Known issues
None.
