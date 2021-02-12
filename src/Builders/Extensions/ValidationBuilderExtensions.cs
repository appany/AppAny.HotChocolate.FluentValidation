using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidationBuilderExtensions
	{
		/// <summary>
		/// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
		/// </summary>
		public static TBuilder SkipValidation<TBuilder>(this CanSkipValidation<TBuilder> builder)
		{
			return builder.SkipValidation(ValidationDefaults.SkipValidation.Skip);
		}

		/// <summary>
		/// Adds default <see cref="InputValidator"/>. See <see cref="ValidationDefaults.InputValidators.Default"/>
		/// </summary>
		public static TBuilder UseDefaultInputValidator<TBuilder>(
			this CanUseInputValidators<TBuilder> builder,
			params InputValidator[] inputValidators)
		{
			return builder.UseInputValidators(
				new InputValidator[] { ValidationDefaults.InputValidators.Default }
					.Concat(inputValidators)
					.ToArray());
		}
	}
}
