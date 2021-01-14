using System;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly ref struct InputValidatorFactoryContext
	{
		public InputValidatorFactoryContext(IMiddlewareContext middlewareContext, Type inputFieldType)
		{
			MiddlewareContext = middlewareContext;
			InputFieldType = inputFieldType;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public Type InputFieldType { get; }
	}
}
