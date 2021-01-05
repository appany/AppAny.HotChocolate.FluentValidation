using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public class FluentValidationOptions
	{
		public IList<ErrorMapper> ErrorMappers { get; set; } = default!;
		public IList<ValidatorFactory> ValidatorFactories { get; set; } = default!;
	}
}
