using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanUseInputValidatorProvidersExtensions
	{
		/// <summary>
		/// Adds default <see cref="InputValidatorProvider"/>. See <see cref="ValidationDefaults.InputValidatorProviders.Default"/>
		/// </summary>
		public static TConfigurator UseDefaultInputValidatorProvider<TConfigurator>(
			this CanUseInputValidatorProviders<TConfigurator> configurator,
			params InputValidatorProvider[] inputValidatorProviders)
		{
			return configurator.UseInputValidatorProviders(
				new InputValidatorProvider[] { ValidationDefaults.InputValidatorProviders.Default }
					.Concat(inputValidatorProviders)
					.ToArray());
		}
	}
}
