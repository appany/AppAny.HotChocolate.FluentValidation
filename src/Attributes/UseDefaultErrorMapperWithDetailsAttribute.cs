namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperWithDetailsAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder argumentValidationBuilder)
    {
      argumentValidationBuilder.UseDefaultErrorMapperWithDetails();
    }
  }
}
