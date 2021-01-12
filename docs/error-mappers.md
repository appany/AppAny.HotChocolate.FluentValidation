
# ErrorMappers

- Error mappers are functions converting validation failure to graphql error
- All methods has overrides to pass more error mappers
- All error mappers executes sequentially and can be composed with other

```json
// .UseDefaultErrorMapper()

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

```json
// .UseDefaultErrorMapperWithDetails()

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

```json
// .UseDefaultErrorMapperWithExtendedDetails()

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
