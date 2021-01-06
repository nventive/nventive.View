# HtmlTextBlockBehavior

## Summary

The `HtmlTextBlockBehavior` class adds HTML parsing capabilities to the TextBlock class.

### Limited HTML support

The behavior only supports the most common HTML tags and only supports attributes on the `<a>` tag.

## Platform support

| Feature                |UWA|Android|iOS|
| -----------------------|:-:|:-:|:-:|
| Display basic HTML | X | X | X |
| OpenPhoneCommand | X | X | X |
| OpenEmailCommand | X | X | X |
| OpenUriCommand   | X | X | X |
| HyperlinkUnderlineStyle   | X | X | X |
| HyperlinkDefaultFontWeight   | X | *X | *X |

## Usage

Add the Umbrella behaviors namespace in the XAML namespace section of your Page/UserControl:

```xml
xmlns:ue="using:Umbrella.View.Extensions"
```

Use the behavior as follows:

```xml	
<TextBlock ue:HtmlTextBlockBehavior.HtmlText="{Binding ElementName=InputTextBlock, Path=Text}"
	ue:HtmlTextBlockBehavior.OpenPhoneCommand="{Binding [Call]}"
	ue:HtmlTextBlockBehavior.OpenEmailCommand="{Binding [OpenEmail]}"
	ue:HtmlTextBlockBehavior.BoldFontFamily="{StaticResource BoldFontFamily01}"
	ue:HtmlTextBlockBehavior.ItalicFontFamily="{StaticResource ItalicFontFamily01}"
	ue:HtmlTextBlockBehavior.BoldItalicFontFamily="{StaticResource BoldItalicFontFamily01}"
	ue:HtmlTextBlockBehavior.RegularFontFamily="{StaticResource RegularFontFamily01}"
	ue:HtmlTextBlockBehavior.SemiBoldFontFamily="{StaticResource SemiBoldFontFamily01}"
	ue:HtmlTextBlockBehavior.HeaderFontSize="15"
	ue:HtmlTextBlockBehavior.HyperlinkUnderlineStyle="Single"
	ue:HtmlTextBlockBehavior.HyperlinkDefaultFontWeight="SemiBold"/>
```

### Commands

#### OpenPhoneCommand and OpenEmailCommand
You can bind command to open hyperlink that contains phone `tel:` or email `mailto:` link via the `OpenPhoneCommand` and `OpenEmailCommand`.

```xml	
<TextBlock ue:HtmlTextBlockBehavior.HtmlText="{Binding ElementName=InputTextBlock, Path=Text}"
	ue:HtmlTextBlockBehavior.OpenPhoneCommand="{Binding [Call]}"
	ue:HtmlTextBlockBehavior.OpenEmailCommand="{Binding [OpenEmail]}"/>
```

```cs
Build(b => b
	.Properties(pb => pb
		.AttachCommand<string>("Call", cb => cb.Execute(async (phoneNumber, ct) => await ExecuteCallCommand(ct, phoneNumber)))
		.AttachCommand<string>("OpenEmail", cb => cb.Execute(async (email, ct) => await ExecuteOpenEmailCommand(ct, email)))
	)
);
```

#### OpenUriCommand
You can now use OpenUriCommand the property to attach a command to your ViewModel whether the user tap an hyperlink. In this case, the ViewModel will receive an `Uri`.
Using an `Uri` is useful, especially since we can easily know what is the type of the link `Uri.Scheme`.

```xml	
<TextBlock ue:HtmlTextBlockBehavior.HtmlText="{Binding ElementName=InputTextBlock, Path=Text}"
	ue:HtmlTextBlockBehavior.OpenUriCommand="{Binding [OpenUrl]}"/>
```

```cs
Build(b => b
	.Properties(pb => pb
		.AttachCommand<Uri>("OpenUrl", cb => cb.Execute(async (url, ct) => await ExecuteOpenUrlCommand(ct, url)))
	)
);

[...]

private async Task ExecuteOpenUrlCommand(CancellationToken ct, Uri url)
{
	switch (url.Scheme)
	{
		case "tel":
			// Handle phone number here
		case "mailto":
			// Handle email here
		case "http":
		case "https":
			// Handle web link here
		default:
			// We could pontentially support many other link types like (sms, skype, etc...)
	}
}
```

## Known issues

* When the input contains the `<i>` tag, sometimes other text runs become italicized.
* When the input contains an `<a>` tag without an `href` attribute, the rest of the input is ignored.
* The `ul` tag is treated as a list item and will insert an empty line with a bullet point.
* The property `HyperlinkDefaultFontWeight` doesn't seems to work properly with custom font on Android and iOS.