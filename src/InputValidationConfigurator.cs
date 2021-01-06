using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IInputValidationConfigurator
	{
		IInputValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
		IInputValidationConfigurator UseValidatorFactories(params InputValidatorFactory[] validatorFactories);
	}

	internal sealed class InputValidationConfigurator : IInputValidationConfigurator
	{
		private readonly IServiceCollection services;

		public InputValidationConfigurator(IServiceCollection services)
		{
			this.services = services;
		}

		public IInputValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<InputValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public IInputValidationConfigurator UseValidatorFactories(params InputValidatorFactory[] validatorFactories)
		{
			services.Configure<InputValidationOptions>(options => options.ValidatorFactories = validatorFactories);

			return this;
		}
	}
}
