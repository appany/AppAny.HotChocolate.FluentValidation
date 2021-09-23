namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseValidatorAttribute<TValidator> : BaseUseValidatorAttribute
    where TValidator : class, IValidator
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      var validationStrategy = TryGetValidationStrategy();

      if (validationStrategy is null)
      {
        argumentValidationBuilder.UseValidator<TValidator>();
      }
      else
      {
        argumentValidationBuilder.UseValidator<TValidator>(validationStrategy);
      }
    }
  }
}
