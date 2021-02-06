using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseFluentValidationMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseFluentValidationMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation] TestPersonInput input)
		{
			return "test";
		}
	}
}
