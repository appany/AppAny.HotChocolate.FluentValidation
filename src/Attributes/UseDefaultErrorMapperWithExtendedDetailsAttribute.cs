namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperWithExtendedDetailsAttribute : FluentValidationAttribute
  {
    public override void Configure(ArgumentValidationBuilder builder)
    {
      builder.UseDefaultErrorMapperWithExtendedDetails();
    }
  }
}
