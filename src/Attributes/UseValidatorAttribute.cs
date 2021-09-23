namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseValidatorAttribute<TValidator> : BaseUseValidatorAttribute
    where TValidator : class, IValidator
  {
    protected internal override void Configure(ArgumentValidationBuilder builder)
    {
      var validationStrategy = TryGetValidationStrategy();

      if (validationStrategy is null)
      {
        builder.UseValidator<TValidator>();
      }
      else
      {
        builder.UseValidator<TValidator>(validationStrategy);
      }
    }
  }
}
