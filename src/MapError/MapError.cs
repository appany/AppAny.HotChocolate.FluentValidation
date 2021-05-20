using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Maps validation result to <see cref="IError" /> properties
  /// </summary>
  public delegate void MapError(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext);
}
