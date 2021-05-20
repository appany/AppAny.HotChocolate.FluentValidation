using System.Threading.Tasks;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class UseInputValidatorExtensions
  {
    /// <summary>
    /// Core extension to generalize input validation
    /// </summary>
    public static ArgumentValidationBuilder UseInputValidator<TInput>(
      this ArgumentValidationBuilder builder,
      GetArgumentValue<TInput> getArgumentValue,
      GetValidationContext<TInput> getValidationContext,
      GetValidatorType getValidatorType,
      GetValidationResult getValidationResult)
    {
      return builder.UseInputValidators(inputValidatorContext =>
      {
        var argumentValue = getArgumentValue(inputValidatorContext);

        if (argumentValue is null)
        {
          return Task.FromResult<ValidationResult?>(null);
        }

        var validationContext = getValidationContext(inputValidatorContext, argumentValue);



        var validatorType = getValidatorType(inputValidatorContext);

        return getValidationResult(inputValidatorContext, validationContext, validatorType);
      });
    }
  }
}
