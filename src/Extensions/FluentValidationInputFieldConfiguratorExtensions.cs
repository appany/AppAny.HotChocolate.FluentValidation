using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class FluentValidationInputFieldConfiguratorExtensions
	{
		public static IFluentValidationInputFieldConfigurator UseValidator<TValidator>(
			this IFluentValidationInputFieldConfigurator configurator)
			where TValidator : class, IValidator
		{
			return configurator.UseValidatorFactories(context => context.ServiceProvider.GetServices<TValidator>());
		}
	}
}
