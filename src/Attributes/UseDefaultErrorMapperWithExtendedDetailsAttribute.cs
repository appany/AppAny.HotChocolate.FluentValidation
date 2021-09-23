namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperWithExtendedDetailsAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder builder)
    {
      builder.UseDefaultErrorMapperWithExtendedDetails();
    }
  }
}
