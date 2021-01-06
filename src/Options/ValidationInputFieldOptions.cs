using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationInputFieldOptions
	{
		public bool SkipValidation { get; set; }

		public IList<ErrorMapper>? ErrorMappers { get; set; }

		public IList<ValidatorFactory>? ValidatorFactories { get; set; }
	}
}
