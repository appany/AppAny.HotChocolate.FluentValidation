using System;
using FluentValidation;
using FluentValidation.Internal;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidatorExtensions
	{
		/// <summary>
		/// Creates new <see cref="InputValidator"/>
		/// </summary>
		public static InputValidator ToInputValidator(this IValidator validator)
		{
			return async (argument, cancellationToken) =>
			{
				var validationContext = new ValidationContext<object>(argument);

				return await validator.ValidateAsync(validationContext, cancellationToken);
			};
		}

		/// <summary>
		/// Creates new <see cref="InputValidator"/> with custom <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static InputValidator ToInputValidatorWithStrategy<TInput>(
			this IValidator<TInput> validator,
			Action<ValidationStrategy<TInput>> validationStrategy)
		{
			return async (argument, cancellationToken) =>
			{
				var validationContext = ValidationContext<TInput>.CreateWithOptions(
					(TInput)argument,
					validationStrategy);

				return await validator.ValidateAsync(validationContext, cancellationToken);
			};
		}
	}
}
