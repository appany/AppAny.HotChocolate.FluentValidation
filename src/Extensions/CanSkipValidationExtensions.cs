namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanSkipValidationExtensions
	{
		/// <summary>
		/// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
		/// </summary>
		public static TBuilder SkipValidation<TBuilder>(
			this CanSkipValidation<TBuilder> builder)
		{
			return builder.SkipValidation(ValidationDefaults.SkipValidation.Skip);
		}
	}
}
