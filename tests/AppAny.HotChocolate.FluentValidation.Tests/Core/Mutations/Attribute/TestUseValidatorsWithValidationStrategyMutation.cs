using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseValidatorsWithValidationStrategyMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseValidatorsWithValidationStrategyMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseValidators(typeof(NotEmptyNameValidator), IncludeProperties = new[] { "Name" })]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
