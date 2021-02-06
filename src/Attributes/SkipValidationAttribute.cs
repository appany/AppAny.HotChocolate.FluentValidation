namespace AppAny.HotChocolate.FluentValidation
{
	public sealed class SkipValidationAttribute : FluentValidationAttribute
	{
		public override void Configure(ArgumentValidationBuilder builder)
		{
			builder.SkipValidation();
		}
	}
}
