using System;
using FluentValidation;
using FluentValidation.Internal;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class UseValidatorWithValidationStrategyExtensions
  {
    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TValidator>(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<object>> validationStrategy)
      where TValidator : class, IValidator
    {
      return builder.UseValidator(typeof(TValidator), validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TValidator>(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
      where TValidator : class, IValidator
    {
      return builder.UseValidator(typeof(TValidator), validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TValidator>(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<object>> validationStrategy)
      where TValidator : class, IValidator
    {
      return builder.UseValidators(typeof(TValidator), validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TValidator>(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
      where TValidator : class, IValidator
    {
      return builder.UseValidators(typeof(TValidator), validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidator(validatorType, (_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidator<object>(validatorType, validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidators(validatorType, (_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<InputValidatorContext, ValidationStrategy<object>> validationStrategy)
    {
      return builder.UseValidators<object>(validatorType, validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(this ArgumentValidationBuilder builder)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidator<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/> with <see cref="ValidationDefaults.ValidationStrategies.Default{TInput}"/> strategy
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(this ArgumentValidationBuilder builder)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidators<TInput, TValidator>(ValidationDefaults.ValidationStrategies.Default);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>, with custom <see cref="ValidationStrategy{TInput}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<TInput>> validationStrategy)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidator<TInput, TValidator>((_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>, with custom <see cref="ValidationStrategy{TInput}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TInput, TValidator>(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidator(typeof(TValidator), validationStrategy);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>, with custom <see cref="ValidationStrategy{TInput}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
      this ArgumentValidationBuilder builder,
      Action<ValidationStrategy<TInput>> validationStrategy)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidators<TInput, TValidator>((_, strategy) => validationStrategy(strategy));
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses all <see cref="TValidator"/> to resolve <see cref="ValidateInput"/>, with custom <see cref="ValidationStrategy{TInput}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TInput, TValidator>(
      this ArgumentValidationBuilder builder,
      Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
      where TValidator : class, IValidator<TInput>
    {
      return builder.UseValidators(typeof(TValidator), validationStrategy);
    }



    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/> with <see cref="ValidationStrategy{T}"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidator<TInput>(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
    {
      return builder.UseInputValidator(
        ValidationDefaults.InputValidators.ArgumentValue<TInput>,
        ValidationDefaults.InputValidators.ValidationContextWithStrategy(validationStrategy)!,
        _ => validatorType,
        ValidationDefaults.InputValidators.Validator);
    }

    /// <summary>
    /// Overrides global <see cref="ValidateInput"/>.
    /// Uses type to resolve <see cref="ValidateInput"/>
    /// </summary>
    public static ArgumentValidationBuilder UseValidators<TInput>(
      this ArgumentValidationBuilder builder,
      Type validatorType,
      Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
    {
      return builder.UseInputValidator(
        ValidationDefaults.InputValidators.ArgumentValue<TInput>,
        ValidationDefaults.InputValidators.ValidationContextWithStrategy(validationStrategy)!,
        _ => validatorType,
        ValidationDefaults.InputValidators.Validators);
    }
  }
}
