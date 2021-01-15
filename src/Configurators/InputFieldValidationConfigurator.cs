using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface InputFieldValidationConfigurator
		: CanSkipValidation<InputFieldValidationConfigurator>,
			CanUseInputValidatorProviders<InputFieldValidationConfigurator>,
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

		public InputFieldValidationConfigurator UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders)
		{
			if (options.InputValidatorProviders is null)
			{
				options.InputValidatorProviders = inputValidatorProviders.ToList();
			}
			else
			{
				foreach (var inputValidatorProvider in inputValidatorProviders)
				{
					options.InputValidatorProviders.Add(inputValidatorProvider);
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
