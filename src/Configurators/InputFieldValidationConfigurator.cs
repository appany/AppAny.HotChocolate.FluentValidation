using System.Linq;

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

		public InputFieldValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] inputValidatorFactories)
		{
			if (options.InputValidatorFactories is null)
			{
				options.InputValidatorFactories = inputValidatorFactories.ToList();
			}
			else
			{
				foreach (var validatorFactory in inputValidatorFactories)
				{
					options.InputValidatorFactories.Add(validatorFactory);
				}
			}

			return this;
		}

		public InputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			if (options.ErrorMappers is null)
			{
				options.ErrorMappers = errorMappers.ToList();
			}
			else
			{
				foreach (var errorMapper in errorMappers)
				{
					options.ErrorMappers.Add(errorMapper);
				}
			}

			return this;
		}
	}
}
