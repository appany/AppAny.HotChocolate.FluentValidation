namespace AppAny.HotChocolate.FluentValidation
{
  public interface CanUseInputValidators<TBuilder>
  {
    /// <summary>
    /// Implementation specific. Can add or override <see cref="ValidateInput"/>
    /// </summary>
    TBuilder UseInputValidators(params ValidateInput[] inputValidators);
  }
}
