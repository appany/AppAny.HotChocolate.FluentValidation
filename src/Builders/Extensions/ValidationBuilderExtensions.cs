using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class ValidationBuilderExtensions
  {
    /// <summary>
    /// Always skips validation <see cref="ValidationDefaults.SkipValidation.Skip"/>
    /// </summary>
    public static TBuilder SkipValidation<TBuilder>(this CanSkipValidation<TBuilder> builder)
    {
      return builder.SkipValidation(ValidationDefaults.SkipValidation.Skip);
    }

    /// <summary>
    /// Uses default <see cref="MapError"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
    /// </summary>
    public static TBuilder UseDefaultErrorMapper<TBuilder>(
      this CanUseErrorMapper<TBuilder> builder,
      MapError? mapError = null)
    {
      return builder.UseErrorMapper((errorBuilder, context) =>
      {
        ValidationDefaults.ErrorMappers.Default(errorBuilder, context);
        mapError?.Invoke(errorBuilder, context);
      });
    }

    /// <summary>
    /// Adds default <see cref="MapError"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
    /// </summary>
    public static TBuilder UseDefaultErrorMapperWithDetails<TBuilder>(
      this CanUseErrorMapper<TBuilder> builder,
      MapError? mapError = null)
    {
      return builder.UseDefaultErrorMapper((errorBuilder, context) =>
      {
        ValidationDefaults.ErrorMappers.Details(errorBuilder, context);
        mapError?.Invoke(errorBuilder, context);
      });
    }

    /// <summary>
    /// Adds default <see cref="MapError"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
    /// </summary>
    public static TBuilder UseDefaultErrorMapperWithExtendedDetails<TBuilder>(
      this CanUseErrorMapper<TBuilder> builder,
      MapError? mapError = null)
    {
      return builder.UseDefaultErrorMapperWithDetails((errorBuilder, context) =>
      {
        ValidationDefaults.ErrorMappers.Extended(errorBuilder, context);
        mapError?.Invoke(errorBuilder, context);
      });
    }

    /// <summary>
    /// Adds default <see cref="ValidateInput"/>. See <see cref="ValidationDefaults.InputValidators.Default"/>
    /// </summary>
    public static TBuilder UseDefaultInputValidator<TBuilder>(
      this CanUseInputValidators<TBuilder> builder,
      params ValidateInput[] inputValidators)
    {
      return builder.UseInputValidators(
        new ValidateInput[] { ValidationDefaults.InputValidators.Default }
          .Concat(inputValidators)
          .ToArray());
    }
  }
}
