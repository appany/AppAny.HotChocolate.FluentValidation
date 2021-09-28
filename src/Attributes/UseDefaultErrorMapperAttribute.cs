namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      argumentValidationBuilder.UseDefaultErrorMapper();
    }
  }
}
