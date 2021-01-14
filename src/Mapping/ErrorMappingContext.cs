using HotChocolate.Types;
using HotChocolate.Resolvers;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly ref struct ErrorMappingContext
	{
		public ErrorMappingContext(
			IMiddlewareContext middlewareContext,
			IInputField inputField,
			ValidationResult validationResult,
			ValidationFailure validationFailure)
		{
			MiddlewareContext = middlewareContext;
			InputField = inputField;
			ValidationResult = validationResult;
			ValidationFailure = validationFailure;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField InputField { get; }
		public ValidationResult ValidationResult { get; }
		public ValidationFailure ValidationFailure { get; }
	}
}
