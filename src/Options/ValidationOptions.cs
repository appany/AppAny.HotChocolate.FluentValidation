using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
  internal sealed class ValidationOptions
  {
    public SkipValidation SkipValidation { get; set; } = ValidationDefaults.SkipValidation.Default;

    public MapError ErrorMapper { get; set; } = ValidationDefaults.ErrorMappers.Default;

    public IList<ValidateInput> InputValidators { get; set; } = new List<ValidateInput>
    {
      ValidationDefaults.InputValidators.Default
    };
  }
}
