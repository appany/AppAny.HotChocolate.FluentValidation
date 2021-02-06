namespace AppAny.HotChocolate.FluentValidation
{
	public sealed class UseDefaultErrorMapperWithDetailsAttribute : FluentValidationAttribute
	{
		public override void Configure(ArgumentValidationBuilder builder)
		{
			builder.UseDefaultErrorMapperWithDetails();
		}
	}
}
