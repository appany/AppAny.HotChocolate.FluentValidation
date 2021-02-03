using HotChocolate.Types;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Context for <see cref="InputValidator"/>
	/// </summary>
	public readonly struct InputValidatorContext
	{
		public InputValidatorContext(IMiddlewareContext middlewareContext, IInputField argument)
		{
			MiddlewareContext = middlewareContext;
			Argument = argument;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField Argument { get; }
	}
}
