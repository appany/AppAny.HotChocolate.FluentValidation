
# ErrorMappers

- Error mappers are functions converting validation failure to graphql error
- All extension methods has overrides to pass more error mappers
- All error mappers executes sequentially and can be composed with other

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
        "inputField": "input",
        "property": "Name",
        "severity": "Error",
        "attemptedValue": ""
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
        "inputField": "input",
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
