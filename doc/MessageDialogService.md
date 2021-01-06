# MessageDialogService

## Summary

The `MessageDialogService` is a service that lets the application display platform specific message dialogs.

## Platform support

| Feature                           | UWA | Android | iOS |
| --------------------------------- |:---:|:-------:|:---:|
| Set a title                       |  X  |    X    |  X  |
| Set a body                        |  X  |    X    |  X  |
| Set a localized title             |  X  |    X    |  X  |
| Set a localized body              |  X  |    X    |  X  |
| Set a **OK** button               |  X  |    X    |  X  |
| Set a **Cancel** button           |  X  |    X    |  X  |
| Set a **Accept** button           |  X  |    X    |  X  |
| Set a **No** button               |  X  |    X    |  X  |
| Set a **Close** button            |  X  |    X    |  X  |
| Set a custom button               |  X  |    X    |  X  |
| Set a destructive button style    |  -  |    -    |  X  |

## Usage

1. The `MessageDialogService` assumes that the application contains specific resource keys for the default button labels.

	* Ok = `MessageDialog_Ok_Label`
	* Cancel = `MessageDialog_Cancel_Label`
	* Close = `MessageDialog_Close_Label`

	Make sure those are present in your application (e.g. in the `.resw` files).

1. On **all platforms**, register the service.
	```csharp
	Container.Register<IMessageDialogService>(c => new MessageDialogService(
		dispatcherScheduler: () => c.ResolveNamed<IScheduler>("UIThread"),
		messageDialogBuilderDelegate: new MessageDialogBuilderDelegate(
			c.Resolve<IResourcesService>.Get
		)
	);
	```

	Umbrella already offers implementations of the `IMessageDialogBuilderDelegate`, you shouldn't have to implement it in your project. This interface interacts directly with the native platform.

	There are implementations for **UAP**, **Uno.UI**, **Xamarin.Forms**.

1. On **all platforms**, use the service (see below for examples).

## Examples

Displaying a simple message that comes from the API, given `message` is the variable containing the message:

```csharp
await _messageService().ShowMessage(ct, mb => mb.Content(message));
```

Let's say you also want a title that comes from resources:

```csharp
await _messageService().ShowMessage(ct, mb => mb
	.Content(message)
	.TitleResource(ResourceKeys.ServerResponseTitle)); 
```

When you add multiple buttons, make sure to add them in the order which they must be displayed, which is generally with the Accept button added last and the Cancel button added first.

Let's say this message is actually a question expecting an Ok/Cancel answer:

```csharp
var answer = await _messageService().ShowMessage(ct, mb => mb
	.Content(message)
	.TitleResource(ResourceKeys.ServerResponseTitle)
    .CancelCommand()
	.OkCommand());

if (answer == MessageDialogResult.Ok)
{
	// ...
}
```

As you guess, anything can come either from a direct string or from resources (adding `Resource` to the extension name):

```csharp
var answer = await _messageService().ShowMessage(ct, mb => mb
	.ContentResource(ResourceKeys.ConfirmProcessBody)
	.TitleResource(ResourceKeys.ConfirmProcessTitle)
    .CommandResource(MessageDialogResult.Cancel, ResourceKeys.AbortCommandLabel)
	.OkCommand());

if (answer == MessageDialogResult.Ok)
{
	// ...
}
```

You don't have to work only with the default `MessageDialogResult` return value. You can use any type.

```csharp
// Given this returns a Player[3]
var topThreePlayers = await this.GetPlayers(ct);

var bestPlayer = await _messageService().ShowMessage<Player>(ct, mb => mb
	.TitleResource(ResourceKeys.SelectBestPlayerTitle)
	.ContentResource(ResourceKeys.SelectBestPlayerBody)
    // No, there's no extension for that ;)
	.Command(topThreePlayers[0], topThreePlayers[0}.Name)
	.Command(topThreePlayers[1], topThreePlayers[1}.Name)
	.Command(topThreePlayers[2], topThreePlayers[2}.Name));
```

Platforms that support a keyboard allows selecting a command when the `Enter` key is pressed. This is called the "default accept" command. Similarily, pressing the `Escape` key will select the command marked as the "default cancel" command. The OkCommand, YesCommand and RetryCommand extensions create a "default accept" command, and CancelCommand, NoCommand and CloseCommand extensions create a "default cancel" command. If you really want to change this behavior, you need to create your commands manually.

```csharp
var answer = await _messageService().ShowMessage(ct, mb => mb
	.Content("What would you vote at the next referendum?")
	.Title("Politics")
	.CancelCommand() // default cancel
	.CommandResource(MessageDialogResult.No, "MessageDialog_No_Label") // neutral
	.AcceptCommand() // default accept
);
```

Some platforms support a destructive style for commands. For example, it allow you to warn the user the action will not be reversible.

```csharp
var answer = await _messageService().ShowMessage(ct, mb => mb
	.TitleResource(ResourceKeys.DeleteTitle)
	.ContentResource(ResourceKeys.DeleteBody)
	.CancelCommand() // default cancel
	.CommandResource(MessageDialogResult.Accept, "MessageDialog_Delete_Label", isDestructive: true) // Delete in destructive style
);
```

## Platform specific limitations

Please remember that there are limits with each platform:
 * Android: The base API removes neutral commands beyond three buttons. The service will instead throw an exception if you add more than three commands.
 * Windows: The API fails with more than three buttons.
 * Windows Phone: The API fails with more than two buttons! If your project has messages with three buttons, you must keep that in mind.
 * iOS: There is no limit. The native UI will show choices in a scrolling container. We decided not to limit either the number of commands, but remember that this pattern is not recommended.

On platforms with hardware back buttons, pressing the back button will either return MessageDialogResult.None or the provided default value for the type you're using (itself defaulting to `default(TResult)`).

Final note: It would be easy to support iOS' `UIAlertControllerStyle.ActionSheet` API from this service. If the need comes up, raise it on UserEcho! (see [this example](https://developer.xamarin.com/recipes/ios/standard_controls/alertcontroller/Images/actionsheet.png)).

## Known issues

None.
