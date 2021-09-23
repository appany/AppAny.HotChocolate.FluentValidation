namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperWithExtendedDetailsAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      argumentValidationBuilder.UseDefaultErrorMapperWithExtendedDetails();
    }
  }
}
