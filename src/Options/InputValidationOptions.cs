using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputValidationOptions
	{
		public IList<ErrorMapper> ErrorMappers { get; set; } = default!;

		public IList<InputValidatorFactory> ValidatorFactories { get; set; } = default!;
	}
}
