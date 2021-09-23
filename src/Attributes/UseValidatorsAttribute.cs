namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseValidatorsAttribute<TValidator> : BaseUseValidatorAttribute
    where TValidator : class, IValidator
  {
    protected internal override void Configure(ArgumentValidationBuilder builder)
    {
      var validationStrategy = TryGetValidationStrategy();

      if (validationStrategy is null)
      {
        builder.UseValidators<TValidator>();
      }
      else
      {
        builder.UseValidators<TValidator>(validationStrategy);
      }
    }
  }
}
