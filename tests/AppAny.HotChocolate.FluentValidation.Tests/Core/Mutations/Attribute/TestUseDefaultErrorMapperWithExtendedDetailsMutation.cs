using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseDefaultErrorMapperWithExtendedDetailsMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseDefaultErrorMapperWithExtendedDetailsMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation, UseDefaultErrorMapperWithExtendedDetails] TestPersonInput input)
		{
			return "test";
		}
	}
}
