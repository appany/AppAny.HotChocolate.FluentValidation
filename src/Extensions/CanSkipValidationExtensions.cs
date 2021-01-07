namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanSkipValidationExtensions
	{
		/// <summary>
		/// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
		/// </summary>
		public static TConfigurator SkipValidation<TConfigurator>(
			this CanSkipValidation<TConfigurator> configurator)
		{
			return configurator.SkipValidation(ValidationDefaults.SkipValidation.Skip);
		}
	}
}
