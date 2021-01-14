using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class TestMutationType : ObjectType
	{
		private readonly Action<IArgumentDescriptor> configure;

		public TestMutationType(Action<IArgumentDescriptor> configure)
		{
			this.configure = configure;
		}

		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field("test")
				.Argument("input", argument => configure.Invoke(argument.Type<TestInputType>()))
				.Resolve("test");
		}
	}
}
