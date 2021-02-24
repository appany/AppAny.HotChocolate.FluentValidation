
# ErrorMappers

- Error mappers are functions converting validation failure to graphql error
- All extension methods has overrides to pass more error mappers
- All error mappers can be composed with other

```cs
public class NotEmptyNameValidator : AbstractValidator<ExampleInput>
{
  public NotEmptyNameValidator()
  {
    RuleFor(input => input.ExampleProperty)
      .NotEmpty()
      .WithMessage("Property is empty")
      .WithErrorCode("ERR0123");
  }
}
```

## .UseDefaultErrorMapper()

```json
{
  "errors": [
    {
      "message": "Property is empty",
      "path": [
        "example"
      ],
      "extensions": {
        "code": "ERR0123"
      }
    }
  ],
  "data": {
    "example": null
  }
}
```

## .UseDefaultErrorMapperWithDetails()

```json
{
  "errors": [
    {
      "message": "Property is empty",
      "path": [
        "example"
      ],
      "extensions": {
        "code": "ERR0123",
        "field": "example",
        "argument": "input",
        "property": "ExampleProperty",
        "severity": "Error"
      }
    }
  ],
  "data": {
    "example": null
  }
}
```

## .UseDefaultErrorMapperWithExtendedDetails()

```json
{
  "errors": [
    {
      "message": "Property is empty",
      "path": [
        "example"
      ],
      "extensions": {
        "code": "ERR0123",
        "field": "example",
        "argument": "input",
        "property": "ExampleProperty",
        "severity": "Error",
        "attemptedValue": "",
        "customState": null,
        "formattedMessagePlaceholderValues": {
          "PropertyName": "ExampleProperty",
          "PropertyValue": ""
        }
      }
    }
  ],
  "data": {
    "example": null
  }
}
```

## Create custom error mappers

```cs
services.AddGraphQLServer()
  .AddFluentValidation(options =>
  {
    options.UseErrorMapper((builder, context) =>
    {
      builder.SetExtension("example", "extension")

      Custom(builder, context);
    });
  })

public static void Custom(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
{
  errorBuilder.SetExtension("custom", "extension");
}
```
