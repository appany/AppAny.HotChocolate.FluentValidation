# Core abstractions

## Delegates

- `SkipValidation` configures skip validation predicate
- `ErrorMapper` maps `FluentValidation` validation result to `HotChocolate` error [click here](examples/error-mappers.md)
- `InputValidator` wraps `IValidator` execution [click here](examples/input-validators.md)
- `InputValidatorProvider` configures conventions to resolve `InputValidator` [click here](examples/input-validator-providers.md)

## Configurators

- `InputValidationConfigurator` configures global validation rules `.AddFluentValidation(...)`
- `InputFieldValidationConfigurator` configures field validation rules `.UseFluentValidation(...)`
