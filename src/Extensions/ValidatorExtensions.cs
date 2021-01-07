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
			return (argument, cancellationToken) =>
			{
				var validationContext = new ValidationContext<object>(argument);

				return validator.ValidateAsync(validationContext, cancellationToken);
			};
		}

		/// <summary>
		/// Creates new <see cref="InputValidator"/> with custom <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static InputValidator ToInputValidatorWithStrategy<TInput>(
			this IValidator validator,
			Action<ValidationStrategy<TInput>> validationStrategy)
		{
			return (argument, cancellationToken) =>
			{
				var validationContext = ValidationContext<TInput>.CreateWithOptions(
					(TInput)argument,
					validationStrategy);

				return validator.ValidateAsync(validationContext, cancellationToken);
			};
		}
	}
}
