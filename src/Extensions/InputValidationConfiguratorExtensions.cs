using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputValidationConfiguratorExtensions
	{
		/// <summary>
		/// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
		/// </summary>
		public static IInputValidationConfigurator SkipValidation(this IInputValidationConfigurator configurator)
		{
			return configurator.SkipValidation(ValidationDefaults.SkipValidation.Skip);
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static IInputValidationConfigurator UseDefaultErrorMapper(
			this IInputValidationConfigurator configurator,
			params ErrorMapper[] errorMappers)
		{
			return configurator.UseErrorMappers(
				new ErrorMapper[] { ValidationDefaults.ErrorMappers.Default }
					.Concat(errorMappers)
					.ToArray());
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static IInputValidationConfigurator UseDefaultErrorMapperWithDetails(
			this IInputValidationConfigurator configurator,
			params ErrorMapper[] errorMappers)
		{
			return configurator.UseErrorMappers(
				new ErrorMapper[] { ValidationDefaults.ErrorMappers.Default, ValidationDefaults.ErrorMappers.Details }
					.Concat(errorMappers)
					.ToArray());
		}

		/// <summary>
		/// Adds default <see cref="InputValidatorFactory"/>. See <see cref="ValidationDefaults.ValidatorFactories.Default"/>
		/// </summary>
		public static IInputValidationConfigurator UseDefaultValidatorFactory(
			this IInputValidationConfigurator configurator,
			params InputValidatorFactory[] inputValidatorFactories)
		{
			return configurator.UseInputValidatorFactories(
				new InputValidatorFactory[] { ValidationDefaults.ValidatorFactories.Default }
					.Concat(inputValidatorFactories)
					.ToArray());
		}
	}
}
