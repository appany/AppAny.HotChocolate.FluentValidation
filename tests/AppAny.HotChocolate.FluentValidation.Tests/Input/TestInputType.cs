using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestInputType : InputObjectType<TestInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<TestInput> descriptor)
		{
			descriptor.Field(x => x.Name);
			descriptor.Field(x => x.Address);
		}
	}
}
