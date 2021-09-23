namespace AppAny.HotChocolate.FluentValidation
{
  public abstract class FluentValidationAttribute : Attribute
  {
    protected internal abstract void Configure(ArgumentValidationBuilder argumentValidationBuilder);
  }
}
