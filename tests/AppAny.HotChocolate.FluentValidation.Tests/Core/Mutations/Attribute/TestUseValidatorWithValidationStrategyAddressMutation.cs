using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseValidatorWithValidationStrategyAddressMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseValidatorWithValidationStrategyAddressMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseValidator(typeof(NotEmptyNameValidator), IncludeProperties = new[] { "Address" })]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
