using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ArgumentValidationBuilderExtensions
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
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<object>> validationStrategy)
			where TValidator : class, IValidator
		{
			return builder.UseValidator(typeof(TValidator), validationStrategy);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TValidator>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
			where TValidator : class, IValidator
		{
			return builder.UseValidator(typeof(TValidator), validationStrategy);
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
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<object>> validationStrategy)
			where TValidator : class, IValidator
		{
			return builder.UseValidators(typeof(TValidator), validationStrategy);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TValidator>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
			where TValidator : class, IValidator
		{
			return builder.UseValidators(typeof(TValidator), validationStrategy);
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
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator(
			this ArgumentValidationBuilder builder,
			Type validatorType,
			Action<ValidationStrategy<object>> validationStrategy)
		{
			return builder.UseValidator(validatorType, (_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/> with <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator(
			this ArgumentValidationBuilder builder,
			Type validatorType,
			Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
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

				var validationContext = ValidationContext<object>.CreateWithOptions(
					argumentValue,
					strategy => validationStrategy(inputValidatorContext, strategy));

				return validator.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted);
			});
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators(
			this ArgumentValidationBuilder builder,
			Type validatorType,
			Action<ValidationStrategy<object>> validationStrategy)
		{
			return builder.UseValidators(validatorType, (_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators(
			this ArgumentValidationBuilder builder,
			Type validatorType,
			Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
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

				var validationContext = ValidationContext<object>.CreateWithOptions(
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

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidators<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<TInput>> validationStrategy)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidator<TInput, TValidator>((_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseInputValidators(inputValidatorContext =>
			{
				var argumentValue = inputValidatorContext
					.MiddlewareContext
					.ArgumentValue<TInput>(inputValidatorContext.Argument.Name);

				if (argumentValue is null)
				{
					return Task.FromResult<ValidationResult?>(null);
				}

				var validator = inputValidatorContext.MiddlewareContext.Services.GetRequiredService<TValidator>();

				var validationContext = ValidationContext<TInput>.CreateWithOptions(
					argumentValue,
					strategy => validationStrategy(inputValidatorContext, strategy));

				return validator.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted);
			});
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<TInput>> validationStrategy)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidators<TInput, TValidator>((_, strategy) => validationStrategy(strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
			where TValidator : class, IValidator<TInput>
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

				var validators = (TValidator[])inputValidatorContext.MiddlewareContext.Services.GetServices<TValidator>();

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
