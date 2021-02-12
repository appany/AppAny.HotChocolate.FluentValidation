using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseDefaultErrorMapperWithDetailsMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseDefaultErrorMapperWithDetailsMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation, UseDefaultErrorMapperWithDetails] TestPersonInput input)
		{
			return "test";
		}
	}
}
