using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class DoubleNotEmptyNameValidator : AbstractValidator<TestPersonInput>
  {
    public const string Message1 = "Name is empty1";
    public const string Message2 = "Name is empty2";

    public DoubleNotEmptyNameValidator()
    {
      RuleFor(input => input.Name)
        .NotEmpty()
        .WithMessage(Message1);

      RuleFor(input => input.Name)
        .NotEmpty()
        .WithMessage(Message2);
    }
  }
}
