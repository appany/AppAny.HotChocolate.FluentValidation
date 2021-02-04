# Input validators

Input validator is **just a delegate**

- Executing one or more validators
- Encapsulating generic and validation strategy calls
- Merging multiple `ValidationResult` into one

```cs
public delegate ValueTask<ValidationResult?> InputValidator(object argument, CancellationToken cancellationToken);
```

By default all `InputValidator` implementations is here `ValidationDefaults.InputValidators`

It is a set of factories with predefined validation scenarios

## Writing custom input validator

Like other parts of this library, you can write your own factory to create `InputValidator`

```cs
public static InputValidator CreateCustomInputValidator()
{
  return context =>
  {
    // Now you need to validate `argument` somehow and return ValidationResult
    return ...;
  }
}
```

Custom input validator usage

```cs
# global
services.AddGraphQL()
  .AddFluentValidation(options =>
  {
    options.UseInputValidators(CreateCustomInputValidator());
  });

# field
descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseInputValidators(CreateCustomInputValidator())
  }));
```
