using System;

namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseValidatorAttribute : BaseUseValidatorAttribute
  {
    public UseValidatorAttribute(Type validatorType)
      : base(validatorType)
    {
    }

    public override void Configure(ArgumentValidationBuilder builder)
    {
      var validationStrategy = TryGetValidationStrategy();

      if (validationStrategy is null)
      {
        builder.UseValidator(ValidatorType);
      }
      else
      {
        builder.UseValidator(ValidatorType, validationStrategy);
      }
    }
  }
}
