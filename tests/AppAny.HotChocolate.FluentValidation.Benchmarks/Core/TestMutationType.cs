using System;
using System.Collections.Generic;
using AppAny.HotChocolate.FluentValidation.Types;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class MyClass
	{
		public string Name { get; set; }
	}

	public class MyClassType : ObjectType<MyClass>
	{
		protected override void Configure(IObjectTypeDescriptor<MyClass> descriptor)
		{
			descriptor.Field(x => x.Name);
		}
	}

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
				.Type(new ValidationResultType<MyClassType>("MyClass", new Dictionary<string, string[]>
				{
					["input"] = new[] { "name" }
				}))
				.Resolve(new MyClass{ Name = "Works!"});

			configure.Invoke(testField);
		}
	}
}
