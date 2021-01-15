# Input validator factories

Input validator factory is **just a delegate**

- Resolving all required `IValidator`
- Creating `InputValidator` from them
- Accessing to `InputValidatorFactoryContext` with all information about execution

```cs
public delegate InputValidator InputValidatorFactory(InputValidatorFactoryContext inputValidatorFactoryContext);
```

By default all `InputValidatorFactory` implementations is here `ValidationDefaults.InputValidatorFactories`

## Writing custom input validator factory

With custom `InputValidatorFactory` you can control `InputValidator` creation, but not execution (it is responsibility of `InputValidator`)

For example you want to get all required `IValidator` instances from custom `IValidatorCache`, not from DI

```cs
public static InputValidatorFactory CreateCustomInputValidatorFactory(IValidatorCache validatorCache)
{
  // InputValidatorFactoryContext
  return context =>
  {
    var cachedValidators = validatorCache.GetValidators(context.InputField.RuntimeType);

    return ValidationDefaults.InputValidators.FromValidators(cachedValidators);
  }
}
```

Usage of custom input validator factory

```cs
# global
services.AddGraphQL()
  .AddFluentValidation(options =>
  {
    options.UseInputValidatorFactories(CreateCustomInputValidatorFactory(validatorCache));
  });

# field
descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseInputValidatorFactories(CreateCustomInputValidatorFactory(validatorCache))
  }));
```
