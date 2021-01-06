using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface IInputFieldValidationConfigurator
	{
		/// <summary>
		/// Sets field options to skip validation
		/// </summary>
		IInputFieldValidationConfigurator SkipValidation();

		/// <summary>
		/// Overrides global validation options. Adds new <see cref="InputValidatorFactory"/> for input field
		/// </summary>
		IInputFieldValidationConfigurator UseValidatorFactories(params InputValidatorFactory[] validatorFactories);

		/// <summary>
		/// Overrides global validation options. Adds new <see cref="ErrorMapper"/> for input field
		/// </summary>
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
