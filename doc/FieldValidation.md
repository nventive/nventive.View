# Field Validation

## Summary
Field validation is a small and simple recipe to validate user input. The idea is that you implement a form without validation, then add validation afterwards by wrapping each field with a `FieldValidationView`.

## Platform support

Feature|UWP|Android|iOS
-|:-:|:-:|:-:
Error visual state|X|X|X
Valid visual state|X|X|X
Normal visual state|X|X|X
Works with any control (TextBox, PasswordBox, CheckBox, Custom, etc.)|X|X|X
Supports _"as you type"_ validation|X|X|X
Supports manual validation|X|X|X
Supports injecting errors (after an API call for example)|-|-|-
Supports visual states during long validation (API driven validation)|-|-|-

## Usage

Lets say you have a login form without input validation. Your ViewModel should have something like this:
```csharp
public LoginPageViewModel()
{
    Build(b => b
        .Properties(pb => pb
            .Attach(Email, () => "")
            .Attach(Password, () => "")
            .AttachCommand("Login", cb => cb.Execute(Login))
        )
    );
}

private IDynamicProperty<string> Email => this.GetProperty<string>();
private IDynamicProperty<string> Password => this.GetProperty<string>();

private async Task Login(CancellationToken ct)
{
    // Proceed with login
}
```
Your Xaml should look like this:

```xml
<TextBox PlaceholderText="Email Address"
        Text="{Binding [Email], Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>

<PasswordBox PlaceholderText="Password"
            Password="{Binding [Password], Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>

<Button Content="Login"
        Command="{Binding [Login]}" />
```

To add input validation, you need to add a FieldValidationViewModel for each field that you want to validate.
You can add those with the `.AttachValidation()` extension on the properties builder.

```csharp
public LoginPageViewModel()
{
    Build(b => b
        .Properties(pb => pb
            .Attach(Email, () => "")
            .Attach(Password, () => "")
            // You add AttachValidation for each field that you want to validate
            .AttachValidation(Email, ValidateEmail)
            .AttachValidation(Password, ValidatePassword)
            .AttachCommand("Login", cb => cb.Execute(Login))
        )
    );
}

private IDynamicProperty<string> Email => this.GetProperty<string>();
private IDynamicProperty<string> Password => this.GetProperty<string>();

// You can implement the rule that you want to validate the input.
// To do produce your validation state, you have access to a few things:
// - the input itself
// - the validation trigger type
// - the previous validation state
private async Task<FieldValidationState> ValidateEmail(CancellationToken ct, string email, ValidationTriggerType trigger, FieldValidationState previousState)
{
   // Validate email input
}

private async Task<FieldValidationState> ValidatePassword(CancellationToken ct, string password, ValidationTriggerType trigger, FieldValidationState previousState)
{
   // Validate password input
}

private async Task Login(CancellationToken ct)
{
    var validationResults = await this.ValidateAllFields(ct);

    if (validationResults.Any(s => s.IsError()))
    {
        // Handle input validation errors. (e.g. show a dialog)
    }

    // Proceed with login
}
```

```xml
<!-- You simply have to wrap the exising controls with a FieldValidationView. -->
<!-- I also strongly suggest you use the LostFocusCommand to trigger a manual validation. -->
<u:FieldValidationView FieldState="{Binding [EmailValidation][ValidationState]}">
    <TextBox PlaceholderText="Email Address"
            Text="{Binding [Email], Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
            ue:ControlExtensions.LostFocusCommand="{Binding [EmailValidation][Validate]}" />
</u:FieldValidationView>

<u:FieldValidationView FieldState="{Binding [PasswordValidation][ValidationState]}">
    <PasswordBox PlaceholderText="Password"
            Password="{Binding [Password], Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
            ue:ControlExtensions.LostFocusCommand="{Binding [PasswordValidation][Validate]}" />
</u:FieldValidationView>

<Button Content="Login"
        Command="{Binding [Login]}" />
```

If you need something more complex than `this.ValidateAllFields(ct)` you can invoke field-by-field validation. There are 2 ways of doing this:
```csharp
// Email can be a Feed<T> or IDynamicProperty<T>
.Attach(Email)
// Attach using a DynamicProperty of FielValidationViewModel
.AttachValidation(EmailValidation, Email, ValidateEmail)

(...)
// This allows you to change the name of the FieldValidationViewModel used for bindings.
private IDynamicProperty<FieldValidationViewModel> EmailValidation => this.GetProperty<FieldValidationViewModel>();

(...)
// Validate by manually
var emailFVVM = await EmailValidation;
var emailValidationState = await emailFVVM.Validate(ct);
```
Or
```csharp
// Email can be a Feed<T> or IDynamicProperty<T>
.Attach(Email)
// Attach using only the input
// This will generate a new dynamic property that is named [NameOfDynamicProperty]Validation
// In this case, EmailValidation
.AttachValidation(Email, ValidateEmail)

(...)
// Validate by manually
var emailFVVM = await this.GetValidationFor(ct, Email);
var emailValidationState = await emailFVVM.Validate(ct);
```

## Known Issues
None

## More Info
[Architecture documentation](..\src\Umbrella.View.Validation\FieldValidation\FieldValidation.Architecture.md)
