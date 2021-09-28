namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Abstracts <see cref="ValidationResult" /> resolving
  /// </summary>
  public delegate Task<ValidationResult?> GetValidationResult(
    InputValidatorContext inputValidatorContext,
    IValidationContext validationContext,
    Type validatorType);
}
