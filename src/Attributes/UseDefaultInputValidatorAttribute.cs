namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultInputValidatorAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      argumentValidationBuilder.UseDefaultInputValidator();
    }
  }
}
