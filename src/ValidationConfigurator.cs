using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IValidationConfigurator
	{
		IValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
		IValidationConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories);
	}

	internal sealed class ValidationConfigurator : IValidationConfigurator
	{
		private readonly IServiceCollection services;

		public ValidationConfigurator(IServiceCollection services)
		{
			this.services = services;
		}

		public IValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<ValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public IValidationConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories)
		{
			services.Configure<ValidationOptions>(options => options.ValidatorFactories = validatorFactories);

			return this;
		}
	}
}
