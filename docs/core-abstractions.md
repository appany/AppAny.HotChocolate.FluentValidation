# Core abstractions

## Delegates

- `SkipValidation` configures skip validation predicate
- `MapError` maps `FluentValidation` validation result to `HotChocolate` error [click here](examples/error-mappers.md)
- `ValidateInput` wraps `IValidator` execution [click here](examples/input-validators.md)

## Builders

- `ValidationBuilder` configures global validation rules `.AddFluentValidation(...)`
- `ArgumentValidationBuilder` configures field validation rules `.UseFluentValidation(...)`
