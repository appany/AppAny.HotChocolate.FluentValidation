using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputValidationOptions
	{
		public SkipValidation SkipValidation { get; set; } = default!;

		public IList<ErrorMapper> ErrorMappers { get; set; } = default!;

		public IList<InputValidatorFactory> InputValidatorFactories { get; set; } = default!;
	}
}
