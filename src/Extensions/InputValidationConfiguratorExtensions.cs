namespace AppAny.HotChocolate.FluentValidation
{
	public static class InputValidationConfiguratorExtensions
	{
		public static IInputValidationConfigurator AddDefaultErrorMapper(this IInputValidationConfigurator configurator)
		{
			return configurator.UseErrorMappers(ValidationDefaults.ErrorMappers.Default);
		}

		public static IInputValidationConfigurator AddDefaultErrorMapperWithDetails(this IInputValidationConfigurator configurator)
		{
			return configurator.UseErrorMappers(
				ValidationDefaults.ErrorMappers.Default,
				ValidationDefaults.ErrorMappers.Details);
		}

		public static IInputValidationConfigurator AddDefaultValidatorFactory(this IInputValidationConfigurator configurator)
		{
			return configurator.UseValidatorFactories(ValidationDefaults.ValidatorFactories.Default);
		}
	}
}
