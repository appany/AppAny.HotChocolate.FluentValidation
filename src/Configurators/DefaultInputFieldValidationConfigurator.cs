using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface InputFieldValidationConfigurator
		: CanSkipValidation<InputFieldValidationConfigurator>,
			CanUseInputValidatorFactories<InputFieldValidationConfigurator>,
			CanUseErrorMappers<InputFieldValidationConfigurator>
	{
	}

	internal sealed class DefaultInputFieldValidationConfigurator : InputFieldValidationConfigurator
	{
		private readonly InputFieldValidationOptions options;

		public DefaultInputFieldValidationConfigurator(InputFieldValidationOptions options)
		{
			this.options = options;
		}

		public InputFieldValidationConfigurator SkipValidation(SkipValidation skipValidation)
		{
			options.SkipValidation = skipValidation;

			return this;
		}

		public InputFieldValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] validatorFactories)
		{
			options.ValidatorFactories ??= new List<InputValidatorFactory>();

			foreach (var validatorFactory in validatorFactories)
			{
				options.ValidatorFactories.Add(validatorFactory);
			}

			return this;
		}

		public InputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
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
