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

## Docs

- [Abstractions](docs/core-abstractions.md)
- [Features](docs/features.md)
- [ErrorMappers](docs/error-mappers.md)
- Examples
  - [Root validator split](docs/examples/root-validator-split.md)
