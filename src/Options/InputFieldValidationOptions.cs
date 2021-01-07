using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputFieldValidationOptions
	{
		public SkipValidation? SkipValidation { get; set; }

		public IList<ErrorMapper>? ErrorMappers { get; set; }

		public IList<InputValidatorFactory>? ValidatorFactories { get; set; }
	}
}
