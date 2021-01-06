using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationOptions
	{
		public IList<ErrorMapper> ErrorMappers { get; set; } = default!;
		public IList<ValidatorFactory> ValidatorFactories { get; set; } = default!;
	}
}
