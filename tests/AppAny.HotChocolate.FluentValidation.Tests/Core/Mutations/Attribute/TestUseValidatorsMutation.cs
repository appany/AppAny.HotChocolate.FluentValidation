using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseValidatorsMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseValidatorsMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation, UseValidators(typeof(NotEmptyNameValidator))] TestPersonInput input)
		{
			return "test";
		}
	}
}
