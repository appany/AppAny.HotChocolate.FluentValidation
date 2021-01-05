using System.Collections.Generic;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	public delegate IEnumerable<IValidator> ValidatorFactory(ValidatorFactoryContext validatorFactoryContext);
}
