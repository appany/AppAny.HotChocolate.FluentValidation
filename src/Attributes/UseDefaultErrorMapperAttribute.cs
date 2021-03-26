namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperAttribute : FluentValidationAttribute
  {
    public override void Configure(ArgumentValidationBuilder builder)
    {
      builder.UseDefaultErrorMapper();
    }
  }
}
