# Core abstractions

## Delegates

- `SkipValidation` configures skip validation predicate
- `ErrorMapper` maps `FluentValidation` validation result to `HotChocolate` error [click here](examples/error-mappers.md)
- `InputValidator` wraps `IValidator` execution [click here](examples/input-validators.md)

## Builders

- `ValidationBuilder` configures global validation rules `.AddFluentValidation(...)`
- `ArgumentValidationBuilder` configures field validation rules `.UseFluentValidation(...)`
