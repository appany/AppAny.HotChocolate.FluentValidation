using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputFieldValidationConfiguratorExtensions
	{
		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => validator.ToInputValidator()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses type to resolve <see cref="InputValidator"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator(
			this InputFieldValidationConfigurator configurator,
			Type validatorType)
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetServices(validatorType)
				.OfType<IValidator>()
				.Select(validator => validator.ToInputValidator()));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, with custom <see cref="ValidationStrategy{TInput}"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetRequiredService<IEnumerable<TValidator>>()
				.Select(validator => validator.ToInputValidatorWithStrategy(strategy)));
		}
	}
}
