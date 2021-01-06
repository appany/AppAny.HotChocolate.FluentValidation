using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IInputFieldValidationConfigurator
	{
		IInputFieldValidationConfigurator SkipValidation();
		IInputFieldValidationConfigurator UseValidatorFactories(params InputValidatorFactory[] validatorFactories);
		IInputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
	}

	internal sealed class InputFieldValidationConfigurator : IInputFieldValidationConfigurator
	{
		private readonly InputFieldValidationOptions options;

		public InputFieldValidationConfigurator(InputFieldValidationOptions options)
		{
			this.options = options;
		}

		public IInputFieldValidationConfigurator SkipValidation()
		{
			options.SkipValidation = true;

			return this;
		}

		public IInputFieldValidationConfigurator UseValidatorFactories(params InputValidatorFactory[] validatorFactories)
		{
			options.ValidatorFactories ??= new List<InputValidatorFactory>();

			foreach (var validatorFactory in validatorFactories)
			{
				options.ValidatorFactories.Add(validatorFactory);
			}

			return this;
		}

		public IInputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
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
