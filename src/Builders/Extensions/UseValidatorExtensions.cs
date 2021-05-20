using System;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class UseValidatorExtensions
  {
    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TValidator>(this ArgumentValidationBuilder builder)
      where TValidator : class, IValidator
    {
      return builder.UseValidator(typeof(TValidator));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TValidator>(this ArgumentValidationBuilder builder)
      where TValidator : class, IValidator
    {
      return builder.UseValidators(typeof(TValidator));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator(
      this ArgumentValidationBuilder builder,
      Type validatorType)
    {
      return builder.UseInputValidator(
        ValidationDefaults.InputValidators.ArgumentValue<object>,
        ValidationDefaults.InputValidators.ValidationContext,
        _ => validatorType,
        ValidationDefaults.InputValidators.Validator);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators(
      this ArgumentValidationBuilder builder,
      Type validatorType)
    {
      return builder.UseInputValidator(
        ValidationDefaults.InputValidators.ArgumentValue<object>,
        ValidationDefaults.InputValidators.ValidationContext,
        _ => validatorType,
        ValidationDefaults.InputValidators.Validators);
    }
  }
}
