using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestSkipValidationMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestSkipValidationMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test([UseFluentValidation, SkipValidation] TestPersonInput input)
    {
      return "test";
    }
  }
}
