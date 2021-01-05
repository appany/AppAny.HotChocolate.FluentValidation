using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IFluentValidationInputFieldConfigurator
	{
		IFluentValidationInputFieldConfigurator SkipValidation();
		IFluentValidationInputFieldConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories);
		IFluentValidationInputFieldConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
	}

	public class FluentValidationInputFieldConfigurator : IFluentValidationInputFieldConfigurator
	{
		private readonly FluentValidationInputFieldOptions options;

		public FluentValidationInputFieldConfigurator(FluentValidationInputFieldOptions options)
		{
			this.options = options;
		}

		public IFluentValidationInputFieldConfigurator SkipValidation()
		{
			options.SkipValidation = true;

			return this;
		}

		public IFluentValidationInputFieldConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories)
		{
			options.ValidatorFactories ??= new List<ValidatorFactory>();

			foreach (var validatorsFactory in validatorFactories)
			{
				options.ValidatorFactories.Add(validatorsFactory);
			}

			return this;
		}

		public IFluentValidationInputFieldConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			options.ErrorMappers ??= new List<ErrorMapper>();

			foreach (var errorMapper in errorMappers)
			{
				options.ErrorMappers.Add(errorMapper);
			}

			return this;
		}
	}
}
