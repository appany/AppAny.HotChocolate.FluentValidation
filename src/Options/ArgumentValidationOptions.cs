using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ArgumentValidationOptions
	{
		public SkipValidation? SkipValidation { get; set; }

		public ErrorMapper? ErrorMapper { get; set; }

		public IList<InputValidatorProvider>? InputValidatorProviders { get; set; }
	}
}
