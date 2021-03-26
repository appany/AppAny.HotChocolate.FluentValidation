namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultInputValidatorAttribute : FluentValidationAttribute
  {
    public override void Configure(ArgumentValidationBuilder builder)
    {
      builder.UseDefaultInputValidator();
    }
  }
}
