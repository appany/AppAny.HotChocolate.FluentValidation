using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputFieldValidationOptions
	{
		public SkipValidation? SkipValidation { get; set; }

		public ICollection<ErrorMapper>? ErrorMappers { get; set; }

		public ICollection<InputValidatorFactory>? ValidatorFactories { get; set; }
	}
}
