using System;
using FluentValidation.Internal;

namespace AppAny.HotChocolate.FluentValidation
{
	public abstract class BaseUseValidatorAttribute : FluentValidationAttribute
	{
		protected BaseUseValidatorAttribute(Type validatorType)
		{
			ValidatorType = validatorType;
		}

		public Type ValidatorType { get; }

		public string[]? IncludeProperties { get; set; }
		public string[]? IncludeRuleSets { get; set; }
		public bool IncludeAllRuleSets { get; set; }
		public bool IncludeRulesNotInRuleSet { get; set; }

		protected Action<ValidationStrategy<object>>? TryGetValidationStrategy()
		{
			var shouldUseValidationStrategy = IncludeProperties is not null
				|| IncludeRuleSets is not null
				|| IncludeAllRuleSets
				|| IncludeRulesNotInRuleSet;

			if (shouldUseValidationStrategy)
			{
				return strategy =>
				{
					if (IncludeProperties is not null)
					{
						strategy.IncludeProperties(IncludeProperties);
					}

					if (IncludeRuleSets is not null)
					{
						strategy.IncludeRuleSets(IncludeRuleSets);
					}

					if (IncludeAllRuleSets)
					{
						strategy.IncludeAllRuleSets();
					}

					if (IncludeRulesNotInRuleSet)
					{
						strategy.IncludeRulesNotInRuleSet();
					}
				};
			}

			return null;
		}
	}
}
