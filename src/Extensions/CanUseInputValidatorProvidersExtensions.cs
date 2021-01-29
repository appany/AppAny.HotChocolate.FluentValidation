using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanUseInputValidatorProvidersExtensions
	{
		/// <summary>
		/// Adds default <see cref="InputValidatorProvider"/>. See <see cref="ValidationDefaults.InputValidatorProviders.Default"/>
		/// </summary>
		public static TBuilder UseDefaultInputValidatorProvider<TBuilder>(
			this CanUseInputValidatorProviders<TBuilder> builder,
			params InputValidatorProvider[] inputValidatorProviders)
		{
			return builder.UseInputValidatorProviders(
				new InputValidatorProvider[] { ValidationDefaults.InputValidatorProviders.Default }
					.Concat(inputValidatorProviders)
					.ToArray());
		}
	}
}
