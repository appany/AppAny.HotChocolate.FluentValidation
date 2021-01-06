using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public delegate IEnumerable<IInputValidator> ValidatorFactory(ValidatorFactoryContext validatorFactoryContext);
}
