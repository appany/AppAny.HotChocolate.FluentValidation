namespace AppAny.HotChocolate.FluentValidation
{
  public interface CanUseErrorMapper<TBuilder>
  {
    /// <summary>
    ///   Implementation specific. Can add or override <see cref="MapError" />
    /// </summary>
    TBuilder UseErrorMapper(MapError errorMapper);
  }
}
