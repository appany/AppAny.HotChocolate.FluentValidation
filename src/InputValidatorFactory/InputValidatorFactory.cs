using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Resolves <see cref="InputValidator"/> by specified convention
	/// </summary>
	/// <param name="inputValidatorFactoryContext"></param>
	public delegate IEnumerable<InputValidator> InputValidatorFactory(
		InputValidatorFactoryContext inputValidatorFactoryContext);
}
