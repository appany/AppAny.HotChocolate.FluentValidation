using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures global validation options
	/// </summary>
	public interface ValidationBuilder
		: CanSkipValidation<ValidationBuilder>,
			CanUseInputValidatorProviders<ValidationBuilder>,
			CanUseErrorMappers<ValidationBuilder>
	{
	}

	internal sealed class DefaultValidationBuilder : ValidationBuilder
	{
		private readonly IServiceCollection services;

		public DefaultValidationBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public ValidationBuilder SkipValidation(SkipValidation skipValidation)
		{
			services.Configure<ValidationOptions>(options => options.SkipValidation = skipValidation);

			return this;
		}

		public ValidationBuilder UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<ValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public ValidationBuilder UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders)
		{
			services.Configure<ValidationOptions>(options => options.InputValidatorProviders = inputValidatorProviders);

			return this;
		}
	}
}
