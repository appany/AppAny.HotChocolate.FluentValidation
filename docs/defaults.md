# Defaults

This library is **not handling** validator registration, so you should add all validators manually or use third-party extensions (e.g. `FluentValidation.AspNetCore.AddValidatorsFromAssemblies`)

## ErrorMapper

By default this library is using `ValidationDefaults.ErrorMappers.Default`

To see error mapper examples [click here](examples/error-mappers.md)

## InputValidator

By default this library is using `ValidationDefaults.InputValidators.Default`

It resolves `ValidateInput` for passed `InputValidatorContext`

To see input validator examples [click here](examples/input-validators.md)

## ValidationStrategy configuration

By default this library is using `ValidationDefaults.ValidationStrategies.Default`

It is doing nothing with validation rules

To see validation strategy examples [click here](examples/validation-strategies.md)

## SkipValidation

By default this library is using `ValidationDefaults.SkipValidation.Default`

It is never skips
