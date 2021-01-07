using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestAttributeMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestAttributeMutation>(x => x.Test(default!)).Type<StringType>();
		}

		public string Test([UseFluentValidation] TestPersonInput input) => "test";
	}
}
