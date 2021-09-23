namespace AppAny.HotChocolate.FluentValidation
{
  public sealed class UseDefaultErrorMapperAttribute : FluentValidationAttribute
  {
    protected internal override void Configure(ArgumentValidationBuilder builder)
    {
      builder.UseDefaultErrorMapper();
    }
  }
}
