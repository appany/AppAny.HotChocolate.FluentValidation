using System;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly ref struct InputValidatorFactoryContext
	{
		public InputValidatorFactoryContext(IMiddlewareContext middlewareContext, IInputField inputField)
		{
			MiddlewareContext = middlewareContext;
			InputField = inputField;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField InputField { get; }
	}
}
