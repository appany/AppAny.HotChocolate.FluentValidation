using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputFieldValidationConfiguratorExtensions
	{
		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidator(
					context.MiddlewareContext.Services.GetRequiredService<TValidator>()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidators<TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidators(
					context.MiddlewareContext.Services.GetServices<TValidator>()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator(
			this InputFieldValidationConfigurator configurator,
			Type validatorType)
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidator(
					(IValidator)context.MiddlewareContext.Services.GetService(validatorType)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidators(
			this InputFieldValidationConfigurator configurator,
			Type validatorType)
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidators(
					(IEnumerable<IValidator>)context.MiddlewareContext.Services.GetServices(validatorType)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static InputFieldValidationConfigurator UseValidators<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidators<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidatorWithStrategy(
					context.MiddlewareContext.Services.GetRequiredService<TValidator>(), strategy));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorProvider"/>.
		/// Uses all <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidators<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseInputValidatorProviders(context =>
				ValidationDefaults.InputValidators.FromValidatorsWithStrategy(
					context.MiddlewareContext.Services.GetServices<TValidator>(), strategy));
		}
	}
}
