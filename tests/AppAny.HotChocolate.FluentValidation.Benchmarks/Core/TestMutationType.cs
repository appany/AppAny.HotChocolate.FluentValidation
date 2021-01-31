using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class TestMutationType : ObjectType
	{
		private readonly Action<IObjectFieldDescriptor> configure;

		public TestMutationType()
		{
			configure = _ =>
			{
			};
		}

		public TestMutationType(Action<IObjectFieldDescriptor> configure)
		{
			this.configure = configure;
		}

		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			var testField = descriptor.Field("test")
				.Resolve("test");

			configure.Invoke(testField);
		}
	}
}
