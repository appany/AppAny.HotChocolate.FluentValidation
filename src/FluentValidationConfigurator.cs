using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IFluentValidationConfigurator
	{
		IFluentValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
		IFluentValidationConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories);
	}

	public class FluentValidationConfigurator : IFluentValidationConfigurator
	{
		private readonly IServiceCollection services;

		public FluentValidationConfigurator(IServiceCollection services)
		{
			this.services = services;
		}

		public IFluentValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<FluentValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public IFluentValidationConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories)
		{
			services.Configure<FluentValidationOptions>(options => options.ValidatorFactories = validatorFactories);

			return this;
		}
	}
}
