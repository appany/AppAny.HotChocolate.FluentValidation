using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public delegate IEnumerable<IInputValidator> InputValidatorFactory(
		InputValidatorFactoryContext inputValidatorFactoryContext);
}
