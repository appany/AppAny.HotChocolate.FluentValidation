
# ErrorMappers

- Error mappers are functions converting validation failure to graphql error
- All extension methods has overrides to pass more error mappers
- All error mappers can be composed with other

## .UseDefaultErrorMapper()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed"
      }
    }
  ],
  "data": {
    "createUser": null
  }
}
```

## .UseDefaultErrorMapperWithDetails()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed",
        "validator": "NotEmptyValidator",
        "argument": "input",
        "property": "Name",
        "severity": "Error"
      }
    }
  ],
  "data": {
    "createUser": null
  }
}
```

## .UseDefaultErrorMapperWithExtendedDetails()

```json
{
  "errors": [
    {
      "message": "Name is empty",
      "path": [
        "createUser"
      ],
      "extensions": {
        "code": "ValidationFailed",
        "validator": "NotEmptyValidator",
        "argument": "input",
        "property": "Name",
        "severity": "Error",
        "attemptedValue": "",
        "customState": null,
        "formattedMessagePlaceholderValues": {
          "PropertyName": "Name",
          "PropertyValue": ""
        }
      }
    }
  ],
  "data": {
    "createUser": null
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
