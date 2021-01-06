using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputValidationOptions
	{
		public SkipValidation SkipValidation { get; set; } = default!;

		public ICollection<ErrorMapper> ErrorMappers { get; set; } = default!;

		public ICollection<InputValidatorFactory> ValidatorFactories { get; set; } = default!;
	}
}
