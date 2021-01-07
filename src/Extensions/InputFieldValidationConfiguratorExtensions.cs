using System;
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
			return configurator.UseValidator(typeof(TValidator));
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
				.Select(validator => InputValidator.FromValidator(validator)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, <see cref="TInput"/> used only for constraint
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="InputValidator"/>, <see cref="TInput"/> used for <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static InputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this InputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => InputValidator.FromValidatorWithStrategy(validator, strategy)));
		}
	}
}
