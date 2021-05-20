using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
  internal sealed class ArgumentValidationOptions
  {
    public SkipValidation? SkipValidation { get; set; }

    public MapError? ErrorMapper { get; set; }

    public IList<ValidateInput>? InputValidators { get; set; }
  }
}
