using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures global validation options
	/// </summary>
	public interface InputValidationBuilder
		: CanSkipValidation<InputValidationBuilder>,
			CanUseInputValidatorProviders<InputValidationBuilder>,
			CanUseErrorMappers<InputValidationBuilder>
	{
	}

	internal sealed class DefaultInputValidationBuilder : InputValidationBuilder
	{
		private readonly IServiceCollection services;

		public DefaultInputValidationBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public InputValidationBuilder SkipValidation(SkipValidation skipValidation)
		{
			services.Configure<InputValidationOptions>(options => options.SkipValidation = skipValidation);

			return this;
		}

		public InputValidationBuilder UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<InputValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public InputValidationBuilder UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders)
		{
			services.Configure<InputValidationOptions>(options => options.InputValidatorProviders = inputValidatorProviders);

			return this;
		}
	}
}
