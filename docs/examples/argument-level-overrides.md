# Argument level overrides

Not all rules, configured globally, suites for corner cases

This library allows to override all globals for field-specific rules

I will use notifications. It is not well designed schema, but can explain thats and whys, when you have same inputs in multiple mutations, etc.

```cs
public record Notification(Guid UserId, string Message);
```

You can send Notifications with `SendNotification` and `SendSystemNotification`

```cs
descriptor.Field(x => x.SendNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation());

descriptor.Field(x => x.SendSystemNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation());
```

Well, there is a problem, mutations are using same input, but each mutation has its own validation rules (it is an artificial restriction, imagine you can't change them to more domain specific Notification and SystemNotification, it is a breaking change)

To resolve this problem and to not to validate each Notification with same IValidator set you can specify which validators should validate input

```cs
descriptor.Field(x => x.SendNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    // HERE
    options.UseValidator<NotificationValidator>();
  }));

descriptor.Field(x => x.SendSystemNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    // HERE
    options.UseValidator<SystemNotificationValidator>();
  }));
```

But what if regular notifications should validate only `Message` with `notification` rule set using all available validators (and system too, why not?), but system only with one?

You can pass not only a concrete validator class, but an interface to resolve all available validators and override `ValidationStrategy<Notification>`

```cs
descriptor.Field(x => x.SendNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    // TValidator = IValidator<Notification> and resolves all validators for notification
    options.UseValidator<Notification, IValidator<Notification>>(strategy =>
    {
      strategy.IncludeProperties(x => x.Message);
      strategy.IncludeRuleSets("notification");
    });
  }));

descriptor.Field(x => x.SendSystemNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseValidator<SystemNotificationValidator>();
  }));
```

Also for system notifications you want to have extended validation details in graphql errors

To achieve this, you can override error mappers

```cs
descriptor.Field(x => x.SendNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseValidator<Notification, IValidator<Notification>>(strategy =>
    {
      strategy.IncludeProperties(x => x.Message);
      strategy.IncludeRuleSets("notification");
    });
  }));

descriptor.Field(x => x.SendSystemNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseValidator<SystemNotificationValidator>();
    // HERE
    options.UseDefaultErrorMapperWithExtendedDetails();
  }));
```

After all only system notifications can skip validation if some condition happens

```cs
descriptor.Field(x => x.SendNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    options.UseValidator<Notification, IValidator<Notification>>(strategy =>
    {
      strategy.IncludeProperties(x => x.Message);
      strategy.IncludeRuleSets("notification");
    });
  }));

descriptor.Field(x => x.SendSystemNotification(default!))
  .Argument("input", argument => argument.UseFluentValidation(options =>
  {
    // HERE
    options.SkipValidation(context => context.MiddlewareContext.Services
      .GetRequiredService<ISomeConditionService>()
      .SomeConditionHappens());

    options.UseValidator<SystemNotificationValidator>();
    options.UseDefaultErrorMapperWithExtendedDetails();
  }));
```

This document has nothing with real world example, but it explains main features of argument level overrides

There are some restrictions by design, most overrides is not supported with attribute-based approach, you can override only validator types or create your own attributes
