using System;
using System.Buffers;
using System.Threading.Tasks;
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

		public static ArgumentValidationBuilder UseValidation<TInput>(
			this ArgumentValidationBuilder builder,
			Func<InputValidatorContext, TInput> argumentValueFactory,
			Func<InputValidatorContext, IValidator[]> validatorsFactory,
			Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy,
			Func<InputValidatorContext, TInput, Action<InputValidatorContext, ValidationStrategy<TInput>>, ValidationContext<TInput>> validationContextFactory,
			Func<InputValidatorContext, ValidationContext<TInput>, IValidator[], Task<ValidationResult?>> validationFactory)
		{
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = argumentValueFactory(context);

				if (argumentValue is null)
				{
					return null;
				}

				var validationContext = validationContextFactory(context, argumentValue, validationStrategy);

				var validators = validatorsFactory(context);

				return await validationFactory(context, validationContext, validators);
			});
		}

		public static ArgumentValidationBuilder UseValidationStrategy<TInput>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
		{
			return builder.UseValidation(
				context => context.MiddlewareContext.ArgumentValue<TInput>(context.Argument.Name),
				context => (IValidator[])context.MiddlewareContext.Services.GetServices(context.Argument.GetGenericValidatorType()),
				validationStrategy,
				(context, input, strategy) => ValidationContext<TInput>.CreateWithOptions(
					input,
					s => strategy(context, s)),
				async (context, validationContext, validators) =>
				{
					ValidationResult? validationResult = null;

					for (var validatorIndex = 0; validatorIndex < validators.Length; validatorIndex++)
					{
						var validator = validators[validatorIndex];

						var validatorResult = await validator
							.ValidateAsync(validationContext, context.MiddlewareContext.RequestAborted)
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

		/// <summary>
		/// Overrides <see cref="ValidationStrategy{T}"/>.
		/// </summary>
		public static ArgumentValidationBuilder __UseValidationStrategy<TInput>(
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
