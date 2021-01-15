# Validation strategies

This library supports strongly typed `FluentValidation.ValidationStrategy` configuration

- By default `ValidationDefaults.ValidationStrategies.Default` is used. It is doing nothing with validation rules
- Validation strategies can be configured only on arguments directly
- Validation strategies is not supporting attribute-based approach out of the box for now

```cs
descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(...))
```

To configure validation strategy use `UseValidator/s` with two generic parameter overloads (because you need to know not only `TValidator`, but `TInput` itself to use with `ValidationStrategy<TInput>` for strongly typed suggestions)

```cs
descriptor.Field(x => x.Example(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    // ValidationStrategy<Example>
    options.UseValidator<Example, ExampleInput>(strategy =>
    {
      strategy.IncludeProperties(input => input.ExampleProperty);
    });
  })))
```

Read more on FluentValidation site <https://docs.fluentvalidation.net/en/latest/rulesets.html>
