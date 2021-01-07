using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class TestInputType : InputObjectType<TestInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<TestInput> descriptor)
		{
			descriptor.Field(x => x.Name);
		}
	}
}
