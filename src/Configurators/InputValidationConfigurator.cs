using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures global validation options
	/// </summary>
	public interface InputValidationConfigurator
		: CanSkipValidation<InputValidationConfigurator>,
			CanUseInputValidatorFactories<InputValidationConfigurator>,
			CanUseErrorMappers<InputValidationConfigurator>
	{
	}

	internal sealed class DefaultInputValidationConfigurator : InputValidationConfigurator
	{
		private readonly IServiceCollection services;

		public DefaultInputValidationConfigurator(IServiceCollection services)
		{
			this.services = services;
		}

		public InputValidationConfigurator SkipValidation(SkipValidation skipValidation)
		{
			services.Configure<InputValidationOptions>(options => options.SkipValidation = skipValidation);

			return this;
		}

		public InputValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<InputValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public InputValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] inputValidatorFactories)
		{
			services.Configure<InputValidationOptions>(options => options.InputValidatorFactories = inputValidatorFactories);

			return this;
		}
	}
}
