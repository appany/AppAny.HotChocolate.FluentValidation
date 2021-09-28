namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Abstracts <see cref="IValidator" /> execution
  /// </summary>
  public delegate Task<ValidationResult?> ValidateInput(InputValidatorContext inputValidatorContext);
}
