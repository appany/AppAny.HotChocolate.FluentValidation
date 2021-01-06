using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputFieldValidationConfiguratorExtensions
	{
		public static IInputFieldValidationConfigurator UseValidator<TValidator>(
			this IInputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidator(validator)));
		}

		public static IInputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this IInputFieldValidationConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
		}

		public static IInputFieldValidationConfigurator UseValidator<TInput, TValidator>(
			this IInputFieldValidationConfigurator configurator,
			Action<ValidationStrategy<TInput>> strategy)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidatorWithStrategy(validator, strategy)));
		}
	}
}
