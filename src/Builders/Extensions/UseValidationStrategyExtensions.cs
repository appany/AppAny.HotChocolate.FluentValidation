using System;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class UseValidationStrategyExtensions
	{
		/// <summary>
		/// Overrides <see cref="ValidationStrategy{T}"/>.
		/// </summary>
		public static ArgumentValidationBuilder UseValidationStrategy(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<object>> validationStrategy)
		{
			return builder.UseValidationStrategy<object>((_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides <see cref="ValidationStrategy{T}"/>.
		/// </summary>
		public static ArgumentValidationBuilder UseValidationStrategy(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
		{
			return builder.UseValidationStrategy<object>(validationStrategy);
		}

		/// <summary>
		/// Overrides <see cref="ValidationStrategy{T}"/>.
		/// </summary>
		public static ArgumentValidationBuilder UseValidationStrategy<TInput>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<TInput>> validationStrategy)
		{
			return builder.UseValidationStrategy<TInput>((_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides <see cref="ValidationStrategy{T}"/>.
		/// </summary>
		public static ArgumentValidationBuilder UseValidationStrategy<TInput>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
		{
			return builder.UseInputValidators(async inputValidatorContext =>
			{
				var argumentValue = inputValidatorContext
					.MiddlewareContext
					.ArgumentValue<TInput>(inputValidatorContext.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validatorType = inputValidatorContext.Argument.GetGenericValidatorType();

				var validators = (IValidator[])inputValidatorContext.MiddlewareContext.Services.GetServices(validatorType);

				var validationContext = ValidationContext<TInput>.CreateWithOptions(
					argumentValue,
					strategy => validationStrategy(inputValidatorContext, strategy));

				ValidationResult? validationResult = null;

				for (var validatorIndex = 0; validatorIndex < validators.Length; validatorIndex++)
				{
					var validator = validators[validatorIndex];

					var validatorResult = await validator
						.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted)
						.ConfigureAwait(false);

					if (validationResult is null)
					{
						validationResult = validatorResult;
					}
					else
					{
						validationResult.MergeFailures(validatorResult);
					}
				}

				return validationResult;
			});
		}
	}
}
