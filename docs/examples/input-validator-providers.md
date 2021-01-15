# Input validator providers

Input validator provider is **just a delegate**

- Resolving all required `IValidator`
- Creating `InputValidator` from them
- Accessing to `InputValidatorProviderContext` with all information about execution

```cs
public delegate InputValidator InputValidatorProvider(InputValidatorProviderContext inputValidatorProviderContext);
```

By default all `InputValidatorProvider` implementations is here `ValidationDefaults.InputValidatorProviders`

## Writing custom input validator provider

With custom `InputValidatorProvider` you can control `InputValidator` creation, but not execution (it is responsibility of `InputValidator`)

For example you want to get all required `IValidator` instances from custom `IValidatorCache`, not from DI

```cs
public static InputValidatorProvider CreateCustomInputValidatorProvider(IValidatorCache validatorCache)
{
  // InputValidatorProviderContext
  return context =>
  {
    var cachedValidators = validatorCache.GetValidators(context.InputField.RuntimeType);

    return ValidationDefaults.InputValidators.FromValidators(cachedValidators);
  }
}
```

Usage of custom input validator provider

```cs
# global
services.AddGraphQL()
  .AddFluentValidation(options =>
  {
    options.UseInputValidatorProviders(CreateCustomInputValidatorProvider(validatorCache));
  });

# field
descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseInputValidatorProviders(CreateCustomInputValidatorProvider(validatorCache))
  }));
```
