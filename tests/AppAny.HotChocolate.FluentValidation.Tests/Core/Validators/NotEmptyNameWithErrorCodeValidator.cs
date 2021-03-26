using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class NotEmptyNameWithErrorCodeValidator : AbstractValidator<TestPersonInput>
  {
    public const string Code = "ERR0123";
    public const string Message = "Name is empty";

    public NotEmptyNameWithErrorCodeValidator()
    {
      RuleFor(input => input.Name)
        .NotEmpty()
        .WithMessage(Message)
        .WithErrorCode(Code);
    }
  }
}
