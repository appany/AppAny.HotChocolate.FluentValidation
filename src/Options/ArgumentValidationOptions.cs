using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ArgumentValidationOptions
	{
		public SkipValidation? SkipValidation { get; set; }

		public IList<ErrorMapper>? ErrorMappers { get; set; }

		public IList<InputValidatorProvider>? InputValidatorProviders { get; set; }
	}
}
