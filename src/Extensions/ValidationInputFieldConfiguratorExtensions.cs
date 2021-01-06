using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidationInputFieldConfiguratorExtensions
	{
		public static IValidationInputFieldConfigurator UseValidator<TValidator>(
			this IValidationInputFieldConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidator(validator)));
		}

		public static IValidationInputFieldConfigurator UseValidator<TInput, TValidator>(
			this IValidationInputFieldConfigurator configurator)
			where TValidator : class, IValidator<TInput>
		{
			return configurator.UseValidatorFactories(context => context
				.ServiceProvider
				.GetServices<TValidator>()
				.Select(validator => IInputValidator.FromValidatorWithStrategy<TInput>(
					validator,
					ValidationDefaults.ValidationStrategies.Default)));
		}

		public static IValidationInputFieldConfigurator UseValidator<TInput, TValidator>(
			this IValidationInputFieldConfigurator configurator,
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
