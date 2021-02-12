using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseValidatorMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseValidatorMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation, UseValidator(typeof(NotEmptyNameValidator))] TestPersonInput input)
		{
			return "test";
		}
	}
}
