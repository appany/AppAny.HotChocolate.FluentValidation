using System;
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
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<object?>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validator = context.MiddlewareContext.Services.GetRequiredService<TValidator>();

				var validationContext = new ValidationContext<object>(argumentValue);

				return await validator
					.ValidateAsync(validationContext, context.MiddlewareContext.RequestAborted)
					.ConfigureAwait(false);
			});
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TValidator>(this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator
		{
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<object?>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validators = (TValidator[])context.MiddlewareContext.Services.GetServices<TValidator>();

				var validationContext = new ValidationContext<object>(argumentValue);

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
						validationResult.MergeValidationResult(validatorResult);
					}
				}

				return validationResult;
			});
		}

		/// <summary>
		/// Overrides global <see cref="InputValidator"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator(
			this ArgumentValidationBuilder builder,
			Type validatorType)
		{
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<object?>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validator = (IValidator)context.MiddlewareContext.Services.GetRequiredService(validatorType);

				var validationContext = new ValidationContext<object>(argumentValue);

				return await validator
					.ValidateAsync(validationContext, context.MiddlewareContext.RequestAborted)
					.ConfigureAwait(false);
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
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<object?>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validators = (IValidator[])context.MiddlewareContext.Services.GetServices(validatorType);

				var validationContext = new ValidationContext<object>(argumentValue);

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
						validationResult.MergeValidationResult(validatorResult);
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
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<TInput>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validator = context.MiddlewareContext.Services.GetRequiredService<TValidator>();

				var validationContext = ValidationContext<TInput>.CreateWithOptions(argumentValue, validationStrategy);

				return await validator
					.ValidateAsync(validationContext, context.MiddlewareContext.RequestAborted)
					.ConfigureAwait(false);
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
			return builder.UseInputValidators(async context =>
			{
				var argumentValue = context.MiddlewareContext.ArgumentValue<TInput>(context.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validators = (TValidator[])context.MiddlewareContext.Services.GetServices<TValidator>();

				var validationContext = ValidationContext<TInput>.CreateWithOptions(argumentValue, validationStrategy);

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
						validationResult.MergeValidationResult(validatorResult);
					}
				}

				return validationResult;
			});
		}
	}
}
