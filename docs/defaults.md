# Defaults

This library is **not handling** validator registration, so you should add all validators manually or use third-party extensions (e.g. `FluentValidation.AspNetCore.AddValidatorsFromAssemblies`)

## ErrorMapper

By default this library using `ValidationDefaults.ErrorMappers.Default`

To see error mapper examples [click here](error-mappers.md)

## InputValidatorFactory

By default this library using `ValidationDefaults.InputValidatorFactories.Default`

It resolves all `IValidator<TInput>` for passed `TInput`

## ValidationStrategy<TInput>

By default this library using `ValidationDefaults.ValidationStrategies.Default`

It is doing nothing with validation rules

## SkipValidation

By default this library using `ValidationDefaults.SkipValidation.Default`

It is never skips
