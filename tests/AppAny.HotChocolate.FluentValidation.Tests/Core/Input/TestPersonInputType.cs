using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestPersonInputType : InputObjectType<TestPersonInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<TestPersonInput> descriptor)
		{
			descriptor.Field(x => x.Name);
			descriptor.Field(x => x.Address);
		}
	}
}
