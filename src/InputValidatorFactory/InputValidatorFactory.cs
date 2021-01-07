using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Resolves <see cref="InputValidator"/> by specified convention
	/// </summary>
	public delegate IEnumerable<InputValidator> InputValidatorFactory(
		InputValidatorFactoryContext inputValidatorFactoryContext);
}
