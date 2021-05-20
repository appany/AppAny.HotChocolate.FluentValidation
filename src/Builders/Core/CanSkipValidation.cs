namespace AppAny.HotChocolate.FluentValidation
{
  public interface CanSkipValidation<TBuilder>
  {
    /// <summary>
    ///   Implementation specific. Can add or override <see cref="SkipValidation" />
    /// </summary>
    TBuilder SkipValidation(SkipValidation skipValidation);
  }
}
