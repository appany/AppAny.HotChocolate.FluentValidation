using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseValidatorWithValidationStrategyMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseValidatorWithValidationStrategyMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseValidator(typeof(NotEmptyNameValidator), IncludeProperties = new[] { "Name" })]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
