# AppAny.HotChocolate.FluentValidation

[![Nuget](https://img.shields.io/nuget/v/AppAny.HotChocolate.FluentValidation.svg)](https://www.nuget.org/packages/AppAny.HotChocolate.FluentValidation) ![Hotchocolate | FluentValidation](https://github.com/appany/AppAny.HotChocolate.FluentValidation/workflows/Hotchocolate%20%7C%20FluentValidation/badge.svg)

Input field `HotChocolate` + `FluentValidation` integration

## Usage

```cs
# Basic
services.AddGraphQLServer()
  .AddFluentValidation();

descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation());

... Example([UseFluentValidation] ExampleInput input) { ... }

# Customizations
services.AddGraphQLServer()
  .AddFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMappers(...)
      .UseInputValidatorFactories(...);
  });

descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMappers(...)
      .UseInputValidatorFactories(...)
      .UseValidator<ExampleInputValidator>()
      .UseValidator<ExampleInput, ExampleInputValidator>()
      .UseValidator<ExampleInput, ExampleInputValidator>(strategy =>
      {
        strategy.IncludeProperties(input => input.ExampleProperty);
        // ...
      });
  }));

... Example([UseFluentValidation(typeof(ExampleInputValidator))] ExampleInput input) { ... }
```

## Abstractions

- `SkipValidation` delegate
  - Configures skip validation predicate

- `ErrorMapper` delegate
  - Maps `FluentValidation` validation result to `HotChocolate` error

- `InputValidator` delegate
  - Wraps `IValidator` execution

- `InputValidatorFactory` delegate
  - Configures conventions to resolve `InputValidator`

## Features

- Global + per-argument multiple `ErrorMapper` configuration
- Global + per-argument multiple `InputValidationFactory` configuration
- Per-argument multiple `InputValidator`
- Per-validator `ValidationStrategy`
- Conditional validation skipping
- Basic attribute-based configuration, code-first preferred

## ErrorMappers

- .UseDefaultErrorMapper()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed"
      }
    }
  ],
  "data": {
    "createUser": null
  }
}
```

- .UseDefaultErrorMapperWithDetails()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed",
        "validator": "NotEmptyValidator",
        "inputField": "input",
        "property": "Name",
        "severity": "Error",
        "attemptedValue": ""
      }
    }
  ],
  "data": {
    "createUser": null
  }
}
```

- .UseDefaultErrorMapperWithExtendedDetails()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed",
        "validator": "NotEmptyValidator",
        "inputField": "input",
        "property": "Name",
        "severity": "Error",
        "attemptedValue": "",
        "customState": null,
        "formattedMessagePlaceholderValues": {
          "PropertyName": "Name",
          "PropertyValue": ""
        }
      }
    }
  ],
  "data": {
    "createUser": null
  }
}
```
