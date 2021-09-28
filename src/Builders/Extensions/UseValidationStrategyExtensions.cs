using FluentValidation.Internal;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class UseValidationStrategyExtensions
  {
    /// <summary>
    ///   Overrides <see cref="ValidationStrategy{T}" />.
    /// </summary>
    public static ArgumentValidationBuilder UseValidationStrategy(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidationStrategy<object>((_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    ///   Overrides <see cref="ValidationStrategy{T}" />.
    /// </summary>
    public static ArgumentValidationBuilder UseValidationStrategy(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidationStrategy<object>(validationStrategy);
    }

    /// <summary>
    ///   Overrides <see cref="ValidationStrategy{T}" />.
    /// </summary>
    public static ArgumentValidationBuilder UseValidationStrategy<TInput>(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<TInput>> validationStrategy)
    {
      return builder.UseValidationStrategy<TInput>((_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    ///   Overrides <see cref="ValidationStrategy{T}" />.
    /// </summary>
    public static ArgumentValidationBuilder UseValidationStrategy<TInput>(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
    {
      return builder.UseInputValidator(
        ValidationDefaults.InputValidators.Steps.ArgumentValue<TInput>,
        ValidationDefaults.InputValidators.Steps.ValidationContextWithStrategy(validationStrategy)!,
        inputValidatorContext => inputValidatorContext.Argument.GetGenericValidatorType(),
        ValidationDefaults.InputValidators.Steps.Validators);
    }
  }
}
