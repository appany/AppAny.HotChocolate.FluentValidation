using HotChocolate.Types;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly ref struct InputValidatorProviderContext
	{
		public InputValidatorProviderContext(IMiddlewareContext middlewareContext, IInputField argument)
		{
			MiddlewareContext = middlewareContext;
			Argument = argument;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField Argument { get; }
	}
}
