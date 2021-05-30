# AppAny.HotChocolate.FluentValidation

[![License](https://img.shields.io/github/license/appany/AppAny.HotChocolate.FluentValidation.svg)](https://github.com/appany/AppAny.HotChocolate.FluentValidation/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/AppAny.HotChocolate.FluentValidation.svg)](https://www.nuget.org/packages/AppAny.HotChocolate.FluentValidation)
[![Downloads](https://img.shields.io/nuget/dt/AppAny.HotChocolate.FluentValidation)](https://www.nuget.org/packages/AppAny.HotChocolate.FluentValidation)
![Tests](https://github.com/appany/AppAny.HotChocolate.FluentValidation/workflows/Tests/badge.svg)
[![codecov](https://codecov.io/gh/appany/AppAny.HotChocolate.FluentValidation/branch/main/graph/badge.svg?token=4ZNLBYOTCI)](https://codecov.io/gh/appany/AppAny.HotChocolate.FluentValidation)


Feature-rich, but simple, fast and memory efficient input field `HotChocolate` + `FluentValidation` integration ⚡️

## 🔧 Installation

```bash
$> dotnet add package AppAny.HotChocolate.FluentValidation
```

## 💡 Features

🚩 You **don't pay** for validation middleware if the field has **no validatable inputs**

🚩 You are **not validating**, and even trying to validate **empty or not marked** as validatable inputs

🚩 Most of extensibility points are just **composable delegates**

🚩 **Fine-tuning** of validation for each field: conditional validation skipping, multiple validators or error mappers per input

🚩 Strongly typed `ValidationStrategy<T>` support

🚩 First-class **attribute-based approach** support

## 🎨 Usage

✅ Add **FluentValidation** [validator](https://docs.fluentvalidation.net/en/latest/start.html)

```cs
public class ExampleInput
{
  public string Example { get; set; }
}

public class ExampleInputValidator : AbstractValidator<ExampleInput>
{
  public ExampleInputValidator()
  {
    RuleFor(input => input.ExampleProperty)
      .NotEmpty()
      .WithMessage("Property is empty");
  }
}
```

✅ Configure **HotChocolate** + **FluentValidation** integration

```cs
# Since FluentValidation.AspNetCore@10.2.0 https://github.com/FluentValidation/FluentValidation/releases/tag/10.2.0
services.AddFluentValidation();

services.AddGraphQLServer()
  .AddFluentValidation();

descriptor.Field(x => x.Example(default!))
  // Explicit over implicit preferred
  // You have to add .UseFluentValidation()/attribute to all arguments requiring validation
  .Argument("input", argument => argument.UseFluentValidation());

... Example([UseFluentValidation] ExampleInput input) { ... }
```

✅ Extend and customize
```cs
services.AddGraphQLServer()
  .AddFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMapper(...)
      .UseInputValidators(...);
  });

descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.SkipValidation(...)
      .UseErrorMapper(...)
      .UseInputValidators(...)
      .UseValidationStrategy(...)
      .UseValidator<ExampleInputValidator>()
      .UseValidator<ExampleInput, ExampleInputValidator>()
      .UseValidator<ExampleInput, ExampleInputValidator>(strategy =>
      {
        strategy.IncludeProperties(input => input.ExampleProperty);
        // ...
      });
  }));

... Example([UseFluentValidation, UseValidator((typeof(ExampleInputValidator))] ExampleInput input) { ... }
```

## 📝 Docs

- 📄 [Abstractions](docs/core-abstractions.md)
- 📄 [Defaults](docs/defaults.md)
- 📄 Examples
  - 📄 [Error mappers](docs/examples/error-mappers.md)
  - 📄 [Validation strategies](docs/examples/validation-strategies.md)
  - 📄 [Input validators](docs/examples/input-validators.md)
  - 📄 [Root validator segregation](docs/examples/root-validator-segregation.md)
  - 📄 [Argument level overrides](docs/examples/argument-level-overrides.md)
  - 📄 [Attribute-based approach](docs/examples/attribute-based-approach.md)

## ♿️ Benchmarks 🚀

🚧 [I **swear** I will check correctness, run these benchmarks on my own environment and only after that I will make conclusions](tests/AppAny.HotChocolate.FluentValidation.Benchmarks/README.md) 🚧

