using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestMutation : ObjectType
  {
    private readonly Action<IObjectFieldDescriptor> configure;

    public TestMutation(Action<IObjectFieldDescriptor> configure)
    {
      this.configure = configure;
    }

    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      var testField = descriptor.Field("test")
        .Type<StringType>()
        .Resolve("test");

      configure.Invoke(testField);
    }
  }
}
