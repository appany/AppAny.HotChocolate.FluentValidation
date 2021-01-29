using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ArgumentValidationBuilderExtensions
	{
		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TValidator>(
			this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidator(
					context.MiddlewareContext.Services.GetRequiredService<TValidator>()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TValidator>(
			this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidators(
					context.MiddlewareContext.Services.GetServices<TValidator>()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator(
			this ArgumentValidationBuilder builder,
			Type validatorType)
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidator(
					(IValidator)context.MiddlewareContext.Services.GetService(validatorType)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators(
			this ArgumentValidationBuilder builder,
			Type validatorType)
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidators(
					(IEnumerable<IValidator>)context.MiddlewareContext.Services.GetServices(validatorType)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
			this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
			this ArgumentValidationBuilder builder)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseValidators<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidatorWithStrategy(
					context.MiddlewareContext.Services.GetRequiredService<TValidator>(), strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
			this ArgumentValidationBuilder builder,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return builder.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidatorsWithStrategy(
					context.MiddlewareContext.Services.GetServices<TValidator>(), strategy));
		}
	}
}
