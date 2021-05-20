namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Abstracts <see cref="TInput" /> argument value resolving
  /// </summary>
  public delegate TInput GetArgumentValue<TInput>(InputValidatorContext inputValidatorContext);
}
