using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseValidatorWithIncludeRuleSetsMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseValidatorWithIncludeRuleSetsMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test(
			[UseFluentValidation, UseValidator(typeof(NotEmptyNameValidator), IncludeRuleSets = new[]{"RuleSet"})]
			TestPersonInput input)
		{
			return "test";
		}
	}
}
