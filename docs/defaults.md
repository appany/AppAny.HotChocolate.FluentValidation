# Defaults

This library is **not handling** validator registration, so you should add all validators manually or use third-party extensions (e.g. `FluentValidation.AspNetCore.AddValidatorsFromAssemblies`)

## ErrorMapper

By default this library using `ValidationDefaults.ErrorMappers.Default`

To see error mapper examples [click here](examples/error-mappers.md)

## InputValidatorProvider

By default this library using `ValidationDefaults.InputValidatorProviders.Default`

It resolves `InputValidator` for passed `InputValidatorProviderContext`

To see input validator provider examples [click here](examples/input-validator-providers.md)

## ValidationStrategy configuration

By default this library using `ValidationDefaults.ValidationStrategies.Default`

It is doing nothing with validation rules

To see validation strategy examples [click here](examples/validation-strategies.md)

## SkipValidation

By default this library using `ValidationDefaults.SkipValidation.Default`

It is never skips
