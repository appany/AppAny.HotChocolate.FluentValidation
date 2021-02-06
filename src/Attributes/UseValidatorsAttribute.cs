using System;

namespace AppAny.HotChocolate.FluentValidation
{
	public sealed class UseValidatorsAttribute : BaseUseValidatorAttribute
	{
		public UseValidatorsAttribute(Type validatorType)
			: base(validatorType)
		{
		}

		public override void Configure(ArgumentValidationBuilder builder)
		{
			var validationStrategy = TryGetValidationStrategy();

			if (validationStrategy is null)
			{
				builder.UseValidators(ValidatorType);
			}
			else
			{
				builder.UseValidators(ValidatorType, validationStrategy);
			}
		}
	}
}
