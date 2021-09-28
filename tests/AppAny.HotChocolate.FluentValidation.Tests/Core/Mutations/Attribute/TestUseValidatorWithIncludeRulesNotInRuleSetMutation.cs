using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class TestUseValidatorWithIncludeRulesNotInRuleSetMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestUseValidatorWithIncludeRulesNotInRuleSetMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, UseValidator<NotEmptyNameValidator>(IncludeRulesNotInRuleSet = true)]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
