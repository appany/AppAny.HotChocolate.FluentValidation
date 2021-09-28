namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Abstracts <see cref="Type" /> validator type resolving
  /// </summary>
  public delegate Type GetValidatorType(InputValidatorContext inputValidatorContext);
}
