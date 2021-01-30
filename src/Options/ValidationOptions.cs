using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationOptions
	{
		public SkipValidation SkipValidation { get; set; } = ValidationDefaults.SkipValidation.Default;

		public IList<ErrorMapper> ErrorMappers { get; set; } = new List<ErrorMapper>
		{
			ValidationDefaults.ErrorMappers.Default
		};

		public IList<InputValidatorProvider> InputValidatorProviders { get; set; } = new List<InputValidatorProvider>
		{
			ValidationDefaults.InputValidatorProviders.Default
		};
	}
}
