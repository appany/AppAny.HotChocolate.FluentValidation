using System.Threading.Tasks;

namespace AppAny.HotChocolate.FluentValidation
{
  public class SkipValidationAttribute : FluentValidationAttribute
  {
    public sealed override void Configure(ArgumentValidationBuilder builder)
    {
      builder.SkipValidation(SkipValidation);
    }

    protected virtual ValueTask<bool> SkipValidation(SkipValidationContext skipValidationContext)
    {
      return ValidationDefaults.SkipValidation.Skip(skipValidationContext);
    }
  }
}
