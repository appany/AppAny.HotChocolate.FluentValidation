namespace AppAny.HotChocolate.FluentValidation
{
  public class SkipValidationAttribute : FluentValidationAttribute
  {
    protected internal sealed override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      argumentValidationBuilder.SkipValidation(SkipValidation);
    }

    protected virtual ValueTask<bool> SkipValidation(SkipValidationContext skipValidationContext)
    {
      return ValidationDefaults.SkipValidation.Skip(skipValidationContext);
    }
  }
}
