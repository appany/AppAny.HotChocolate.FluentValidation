# Root validator split

Imagine you have a User

```cs
public record User(string FirstName, string LastName, string Address);
```

You want to validate address separately, to have a compact and readable validator, so you create `UserValidator`

```cs
public class UserValidator : AbstractValidator<User>
{
  public UserValidator()
  {
    ...

    RuleFor(x => x.Address)
      .SetValidator(new DefaultUserAddressValidator());
  }
}
```

And set address validator to `DefaultUserAddressValidator`

```cs
public class DefaultUserAddressValidator : AbstractValidator<string>
{
  public DefaultUserAddressValidator()
  {
    RuleFor(x => x)
      .Must(...)
      .WithMessage(...)
  }
}
```

But it is tightly couple two validators. What if you have another way to validate address? Or to validate address with first name?

```cs
public class CustomUserAddressValidator : AbstractValidator<string>
{
  public DefaultUserAddressValidator()
  {
    RuleFor(x => x)
      .Must(...)
      .WithMessage(...);
  }
}
```

To override address validator you should pass it as a constructor argument (or maybe null?)

```cs
public class UserValidator : AbstractValidator<User>
{
  public UserValidator(IValidator<string> validator)
  {
    ...

    RuleFor(x => x.Address)
      // Some null checks and if statements omitted
      .SetValidator(validator);
  }
}
```

But how you can do that with DI? Which `IValidator<string>` you should resolve? There are some solutions (separate user validators, factories, manual DI, etc.), but this library supports multiple root validators and automatically calls them sequentially, converting failures to useful GraphQL errors

To use this feature just call multiple `UseValidator` with multiple **User** validators

```cs
public class DefaultAddressUserValidator : AbstractValidator<User>
{
  public DefaultAddressUserValidator()
  {
    RuleFor(x => x.Address)
      ...
      ;
  }
}

public class CustomAddressUserValidator : AbstractValidator<User>
{
  public CustomAddressUserValidator()
  {
    RuleFor(x => x.Address)
      ...
      ;
  }
}

# Usage
descriptor.Field(x => x.CreateUser(default!))
  .Argument("input", x => x.UseFluentValidation(options =>
  {
    options.UseValidator<UserValidator>()
      // Use required validator without UserValidator modification
      .UseValidator<DefaultAddressUserValidator>() or/and .UseValidator<CustomAddressUserValidator>();
  }));
# or
... CreateUser([UseFluentValidation(typeof(UserValidator), typeof(DefaultAddressUserValidator))] User user)

descriptor.Field(x => x.CreateExternalUser(default!))
  .Argument("input", x => x.UseFluentValidation(options =>
  {
    options.UseValidator<UserValidator>()
      // For external users we will use only Custom address validator
      .UseValidator<CustomAddressUserValidator>();
  }));
# or
... CreateUser([UseFluentValidation(typeof(UserValidator), typeof(CustomAddressUserValidator))] User user)
```

This is not a silver bullet, this library handles only root validators and **only** static validator sequences. If you want dynamically change validation rules (feature flags?) prefer another solution
