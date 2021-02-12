using HotChocolate.Types;
using HotChocolate.Resolvers;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Context for <see cref="ErrorMapper"/>
	/// </summary>
	public readonly ref struct ErrorMappingContext
	{
		public ErrorMappingContext(
			IMiddlewareContext middlewareContext,
			IInputField argument,
			global::FluentValidation.Results.ValidationResult validationResult,
			ValidationFailure validationFailure)
		{
			MiddlewareContext = middlewareContext;
			Argument = argument;
			ValidationResult = validationResult;
			ValidationFailure = validationFailure;
		}

		public IMiddlewareContext MiddlewareContext { get; }
		public IInputField Argument { get; }
		public global::FluentValidation.Results.ValidationResult ValidationResult { get; }
		public ValidationFailure ValidationFailure { get; }
	}
}
