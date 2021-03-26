using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseDefaultInputValidatorMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseDefaultInputValidatorMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseDefaultInputValidator]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
