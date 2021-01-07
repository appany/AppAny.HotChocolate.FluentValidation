using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class TestMutationWithValidationType : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field("test")
				.Argument("input", argument => argument.Type<TestInputType>().UseFluentValidation())
				.Resolve("test");
		}
	}
}
