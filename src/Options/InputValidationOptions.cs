using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputValidationOptions
	{
		public SkipValidation SkipValidation { get; set; } = default!;

		public IList<ErrorMapper> ErrorMappers { get; set; } = default!;

		public IList<InputValidatorProvider> InputValidatorProviders { get; set; } = default!;
	}
}
