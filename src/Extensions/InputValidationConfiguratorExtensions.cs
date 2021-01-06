using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputValidationConfiguratorExtensions
	{
		/// <summary>
		/// Adds default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static IInputValidationConfigurator AddDefaultErrorMapper(
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
		public static IInputValidationConfigurator AddDefaultErrorMapperWithDetails(
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
		public static IInputValidationConfigurator AddDefaultValidatorFactory(
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
