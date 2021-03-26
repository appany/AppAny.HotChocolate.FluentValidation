using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseValidatorsWithValidationStrategyAddressMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseValidatorsWithValidationStrategyAddressMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseValidators(typeof(NotEmptyNameValidator), IncludeProperties = new[] { "Address" })]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
