using HotChocolate.Types;
using HotChocolate.Resolvers;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly ref struct ErrorMappingContext
	{
		public ErrorMappingContext(
			IMiddlewareContext middlewareContext,
			IInputField argument,
			ValidationResult validationResult,
			ValidationFailure validationFailure)
		{
			MiddlewareContext = middlewareContext;
			Argument = argument;
			ValidationResult = validationResult;
			ValidationFailure = validationFailure;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField Argument { get; }
		public ValidationResult ValidationResult { get; }
		public ValidationFailure ValidationFailure { get; }
	}
}
