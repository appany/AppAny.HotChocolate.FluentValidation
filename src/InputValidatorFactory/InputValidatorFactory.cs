using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Resolves <see cref="IInputValidator"/> by specified convention
	/// </summary>
	/// <param name="inputValidatorFactoryContext"></param>
	public delegate IEnumerable<IInputValidator> InputValidatorFactory(
		InputValidatorFactoryContext inputValidatorFactoryContext);
}
