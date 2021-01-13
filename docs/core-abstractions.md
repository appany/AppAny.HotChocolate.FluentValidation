# Core abstractions

## Delegates

- `SkipValidation` configures skip validation predicate
- `ErrorMapper` maps `FluentValidation` validation result to `HotChocolate` error [click here](error-mappers.md)
- `InputValidator` wraps `IValidator` execution
- `InputValidatorFactory` configures conventions to resolve `InputValidator`

## Configurators

- `InputValidationConfigurator` configures global validation rules `.AddFluentValidation(...)`
- `InputFieldValidationConfigurator` configures field validation rules `.UseFluentValidation(...)`
