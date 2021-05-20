using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Abstracts <see cref="IValidationContext" /> argument value resolving
  /// </summary>
  public delegate IValidationContext GetValidationContext<TInput>(
    InputValidatorContext inputValidatorContext,
    TInput argumentValue);
}
