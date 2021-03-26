using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestPersonInputType : InputObjectType<TestPersonInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<TestPersonInput> descriptor)
    {
      descriptor.Field(field => field.Name);
      descriptor.Field(field => field.Address);
    }
  }
}
