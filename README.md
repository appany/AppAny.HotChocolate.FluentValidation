# AppAny.HotChocolate.FluentValidation

[![Nuget](https://img.shields.io/nuget/v/AppAny.HotChocolate.FluentValidation.svg)](https://www.nuget.org/packages/AppAny.HotChocolate.FluentValidation) ![Hotchocolate | FluentValidation](https://github.com/appany/AppAny.HotChocolate.FluentValidation/workflows/Hotchocolate%20%7C%20FluentValidation/badge.svg)

Input field `HotChocolate` + `FluentValidation` integration

## Disclaimer

This library is a rework of internal package inside @appany

## Features

- You don't pay for validation middleware if the field has no validatable inputs
- You are not validating, and even trying to validate empty or not marked as validatable inputs
- Most of extensibility points is just a composable delegates
- Fine-tuning of validation for each field: conditional validation skipping, multiple validators or error mappers per input
- Strongly typed `ValidationStrategy<T>` support
- Basic attribute-based support

## Docs

- [Abstractions](docs/core-abstractions.md)
- [Defaults](docs/defaults.md)
- Examples
  - [Error mappers](docs/examples/error-mappers.md)
  - [Validation strategies](docs/examples/validation-strategies.md)
  - [Input validators](docs/examples/input-validators.md)
  - [Input validator providers](docs/examples/input-validator-providers.md)
  - [Root validator segregation](docs/examples/root-validator-segregation.md)
  - [Argument level overrides](docs/examples/argument-level-overrides.md)

## Usage

```cs
# Basic
services.AddGraphQLServer()
  .AddFluentValidation();

descriptor.Field(x => x.Example(default!))
  // Explicit over implicit preferred, you have to add .UseFluentValidation() to all arguments requiring validation
  .Argument("input", argument => argument.UseFluentValidation());

... Example([UseFluentValidation] ExampleInput input) { ... }

# Customizations
services.AddGraphQLServer()
  .AddFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMapper(...)
      .UseInputValidatorProviders(...);
  });

descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMapper(...)
      .UseInputValidatorProviders(...)
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

## Benchmarks

- [I **swear** I will check correctness, run these benchmarks on my own environment and only after that I will make conclusions](tests/AppAny.HotChocolate.FluentValidation.Benchmarks/README.md)
