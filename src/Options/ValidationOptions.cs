using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationOptions
	{
		public SkipValidation SkipValidation { get; set; } = ValidationDefaults.SkipValidation.Default;

		public ErrorMapper ErrorMapper { get; set; } = ValidationDefaults.ErrorMappers.Default;

		public IList<InputValidator> InputValidators { get; set; } = new List<InputValidator>
		{
			ValidationDefaults.InputValidators.Default
		};
	}
}
