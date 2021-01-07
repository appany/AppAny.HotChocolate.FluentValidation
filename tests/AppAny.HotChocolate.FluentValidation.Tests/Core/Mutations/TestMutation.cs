using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestMutation : ObjectType
	{
		private readonly Action<IArgumentDescriptor> configure;

		public TestMutation(Action<IArgumentDescriptor> configure)
		{
			this.configure = configure;
		}

		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field("test")
				.Type<StringType>()
				.Argument("input", arg =>
				{
					arg.Type<NonNullType<TestPersonInputType>>();
					configure(arg);
				})
				.Resolve("test");
		}
	}
}
