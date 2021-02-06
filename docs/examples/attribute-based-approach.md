# Attribute-based approach

Use default input validator

```cs
... Example([UseFluentValidation] ExampleInput input) { ... }
```

Use single validator type for single validator

```cs
... Example([UseFluentValidation, UseValidator(typeof(ExampleInputValidator))] ExampleInput input) { ... }
```

Use single validator type for multiple validators

```cs
... Example([UseFluentValidation, UseValidators(typeof(IValidator<ExampleInput>))] ExampleInput input) { ... }
```

Use validator with custom validation strategy

```cs
... Example([
  UseFluentValidation,
  UseValidator(
    typeof(ExampleInputValidator),
    IncludeProperties=new[]{"ExampleProperty"},
    IncludeRuleSets=new[]{"FastValidation"})
  ] ExampleInput input) { ... }
```

Use validators with custom [validation strategy](validation-strategies.md)

```cs
... Example([
  UseFluentValidation,
  UseValidators(
    typeof(ExampleInputValidator),
    IncludeAllRuleSets=true,
    IncludeRulesNotInRuleSet=true)
  ] ExampleInput input) { ... }
```

Use default [input validator](input-validators.md)

```cs
... Example([UseFluentValidation, UseDefaultInputValidator)] ExampleInput input) { ... }
```

Use default [error mapper](error-mappers.md)

```cs
... Example([UseFluentValidation, UseDefaultErrorMapper)] ExampleInput input) { ... }
```

Use default [error mapper](error-mappers.md) with details

```cs
... Example([UseFluentValidation, UseDefaultErrorMapperWithDetails)] ExampleInput input) { ... }
```

Use default [error mapper](error-mappers.md) with extended details

```cs
... Example([UseFluentValidation, UseDefaultErrorMapperWithExtendedDetails)] ExampleInput input) { ... }
```

Skip validation

```cs
... Example([UseFluentValidation, SkipValidation)] ExampleInput input) { ... }
```
