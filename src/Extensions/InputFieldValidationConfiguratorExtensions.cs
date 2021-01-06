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
		/// Overrides global <see cref="ValidationDefaults.SkipValidation.Default"/>.
		/// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
		/// </summary>
		public static IInputFieldValidationConfigurator SkipValidation(this IInputFieldValidationConfigurator configurator)
		{
			return configurator.SkipValidation(ValidationDefaults.SkipValidation.Skip);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="IInputValidator"/>
		/// </summary>
		public static IInputFieldValidationConfigurator UseValidator<TValidator>(
			this IInputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidator(validator)));
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="IInputValidator"/>, <see cref="TInput"/> used only for constraint
		/// </summary>
		public static IInputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this IInputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		/// <summary>
		/// Overrides global <see cref="InputValidatorFactory"/>.
		/// Uses <see cref="TValidator"/> to resolve <see cref="IInputValidator"/>, <see cref="TInput"/> used for <see cref="ValidationStrategy{T}"/>
		/// </summary>
		public static IInputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this IInputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseInputValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidatorWithStrategy(validator, strategy)));
		}
	}
}
