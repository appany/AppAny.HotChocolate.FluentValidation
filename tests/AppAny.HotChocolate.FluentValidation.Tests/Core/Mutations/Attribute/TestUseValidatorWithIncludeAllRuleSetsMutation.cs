using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class TestUseValidatorWithIncludeAllRuleSetsMutation : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Field<TestUseValidatorWithIncludeAllRuleSetsMutation>(
				field => field.Test(default!)).Type<StringType>();
		}

		public string Test(
			[UseFluentValidation, UseValidator(typeof(NotEmptyNameValidator), IncludeAllRuleSets = true)]
			TestPersonInput input)
		{
			return "test";
		}
	}
}
