namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Checks for validation skip
  /// </summary>
  public delegate ValueTask<bool> SkipValidation(SkipValidationContext skipValidationContext);
}
