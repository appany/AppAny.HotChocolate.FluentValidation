using FluentValidation.Internal;

namespace AppAny.HotChocolate.FluentValidation
{
  public abstract class BaseUseValidatorAttribute : FluentValidationAttribute
  {
    /// <summary>
    ///   Include property names. Passed to <see cref="ValidationStrategy{T}.IncludeProperties(string[])"/>
    /// </summary>
    public string[]? IncludeProperties { get; set; }

    /// <summary>
    ///   Include rule sets. Passed to <see cref="ValidationStrategy{T}.IncludeRuleSets(string[])"/>
    /// </summary>
    public string[]? IncludeRuleSets { get; set; }

    /// <summary>
    ///   Include all rule sets. Use <see cref="ValidationStrategy{T}.IncludeAllRuleSets()"/>
    /// </summary>
    public bool IncludeAllRuleSets { get; set; }

    /// <summary>
    ///   Include rules not in rule set. Use <see cref="ValidationStrategy{T}.IncludeRulesNotInRuleSet()"/>
    /// </summary>
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
