using HotChocolate.Types;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly struct SkipValidationContext
	{
		public SkipValidationContext(
			IMiddlewareContext middlewareContext,
			IInputField argument)
		{
			MiddlewareContext = middlewareContext;
			Argument = argument;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField Argument { get; }
	}
}
