namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputValidationConfiguratorExtensions
	{
		/// <summary>
		/// Adds default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static IInputValidationConfigurator AddDefaultErrorMapper(this IInputValidationConfigurator configurator)
		{
			return configurator.UseErrorMappers(ValidationDefaults.ErrorMappers.Default);
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static IInputValidationConfigurator AddDefaultErrorMapperWithDetails(
			this IInputValidationConfigurator configurator)
		{
			return configurator.UseErrorMappers(
				ValidationDefaults.ErrorMappers.Default,
				ValidationDefaults.ErrorMappers.Details);
		}

		/// <summary>
		/// Adds default <see cref="InputValidatorFactory"/>. See <see cref="ValidationDefaults.ValidatorFactories.Default"/>
		/// </summary>
		/// <param name="configurator"></param>
		/// <returns></returns>
		public static IInputValidationConfigurator AddDefaultValidatorFactory(
			this IInputValidationConfigurator configurator)
		{
			return configurator.UseInputValidatorFactories(ValidationDefaults.ValidatorFactories.Default);
		}
	}
}
