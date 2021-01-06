# AppAny.HotChocolate.FluentValidation

[![Nuget](https://img.shields.io/nuget/v/AppAny.HotChocolate.FluentValidation.svg)](https://www.nuget.org/packages/AppAny.HotChocolate.FluentValidation) ![Hotchocolate | FluentValidation](https://github.com/appany/AppAny.HotChocolate.FluentValidation/workflows/Hotchocolate%20%7C%20FluentValidation/badge.svg)

Input field `HotChocolate` + `FluentValidation` validation integration

## Abstractions

- `ErrorMapper` delegate
  - Maps `FluentValidation` validation result to `HotChocolate` error

- `InputValidationFactory` delegate
  - Configures conventions to resolve `FluentValidation` validators

## Features

- Global + per-argument multiple `ErrorMapper` configuration support
- Global + per-argument multiple `InputValidationFactory` configuration support
- Per-argument multiple `IValidator` support
- Per-validator `ValidationStrategy` support
- Conditionally skipping validation support

## Usage

```cs
# Basic
services.AddGraphQLServer()
  .AddFluentValidation();

descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation());

# Customizations
services.AddGraphQLServer()
  .AddFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMappers(...)
      .UseInputValidatorFactories(...)
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
      })
  }))
```
