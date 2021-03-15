using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class UseValidatorExtensions
	{
		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TValidator>(this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator
		{
			return builder.UseValidator(typeof(TValidator));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TValidator>(this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator
		{
			return builder.UseValidators(typeof(TValidator));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator(
			this ArgumentValidationBuilder builder,
			Type validatorType)
		{
			return builder.UseInputValidators(inputValidatorContext =>
			{
				var argumentValue = inputValidatorContext
					.MiddlewareContext
					.ArgumentValue<object?>(inputValidatorContext.Argument.Name);

				if (argumentValue is null)
				{
					return Task.FromResult<ValidationResult?>(null);
				}

				var validator = (IValidator)inputValidatorContext.MiddlewareContext.Services.GetRequiredService(validatorType);

				var validationContext = new ValidationContext<object>(argumentValue);

				return validator.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted);
			});
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators(
			this ArgumentValidationBuilder builder,
			Type validatorType)
		{
			return builder.UseInputValidators(async inputValidatorContext =>
			{
				var argumentValue = inputValidatorContext
					.MiddlewareContext
					.ArgumentValue<object?>(inputValidatorContext.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validators = (IValidator[])inputValidatorContext.MiddlewareContext.Services.GetServices(validatorType);

				var validationContext = new ValidationContext<object>(argumentValue);

				ValidationResult? validationResult = null;

				for (var validatorIndex = 0; validatorIndex < validators.Length; validatorIndex++)
				{
					var validator = validators[validatorIndex];

					var validatorResult = await validator
						.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted)
						.ConfigureAwait(false);

					validationResult = validatorResult;
				}

				return validationResult;
			});
		}
	}
}
