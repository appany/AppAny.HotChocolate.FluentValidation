using System;

namespace AppAny.HotChocolate.FluentValidation
{
	public abstract class FluentValidationAttribute : Attribute
	{
		public abstract void Configure(ArgumentValidationBuilder builder);
	}
}
