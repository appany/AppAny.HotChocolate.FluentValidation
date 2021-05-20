namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  /// Configures global validation options
  /// </summary>
  public interface ValidationBuilder
    : CanSkipValidation<ValidationBuilder>,
      CanUseInputValidators<ValidationBuilder>,
      CanUseErrorMapper<ValidationBuilder>
  {
  }

  internal sealed class DefaultValidationBuilder : ValidationBuilder
  {
    private readonly ValidationOptions options;

    public DefaultValidationBuilder(ValidationOptions options)
    {
      this.options = options;
    }

    public ValidationBuilder SkipValidation(SkipValidation skipValidation)
    {
      options.SkipValidation = skipValidation;

      return this;
    }

    public ValidationBuilder UseErrorMapper(MapError mapError)
    {
      options.ErrorMapper = mapError;

      return this;
    }

    public ValidationBuilder UseInputValidators(params ValidateInput[] inputValidators)
    {
      options.InputValidators = inputValidators;

      return this;
    }
  }
}
