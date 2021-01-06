using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IValidationInputFieldConfigurator
	{
		IValidationInputFieldConfigurator SkipValidation();
		IValidationInputFieldConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories);
		IValidationInputFieldConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
	}

	internal sealed class ValidationInputFieldConfigurator : IValidationInputFieldConfigurator
	{
		private readonly ValidationInputFieldOptions options;

		public ValidationInputFieldConfigurator(ValidationInputFieldOptions options)
		{
			this.options = options;
		}

		public IValidationInputFieldConfigurator SkipValidation()
		{
			options.SkipValidation = true;

			return this;
		}

		public IValidationInputFieldConfigurator UseValidatorFactories(params ValidatorFactory[] validatorFactories)
		{
			options.ValidatorFactories ??= new List<ValidatorFactory>();

			foreach (var validatorFactory in validatorFactories)
			{
				options.ValidatorFactories.Add(validatorFactory);
			}

			return this;
		}

		public IValidationInputFieldConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
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
