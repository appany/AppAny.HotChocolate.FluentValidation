using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public class FluentValidationInputFieldOptions
	{
		public bool SkipValidation { get; set; }

		public IList<ErrorMapper>? ErrorMappers { get; set; }

		public IList<ValidatorFactory>? ValidatorFactories { get; set; }
	}
}
