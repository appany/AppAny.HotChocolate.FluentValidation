namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseValidatorsAttribute<TValidator> : BaseUseValidatorAttribute
    where TValidator : class, IValidator
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      var validationStrategy = TryGetValidationStrategy();

      if (validationStrategy is null)
      {
        argumentValidationBuilder.UseValidators<TValidator>();
      }
      else
      {
        argumentValidationBuilder.UseValidators<TValidator>(validationStrategy);
      }
    }
  }
}
