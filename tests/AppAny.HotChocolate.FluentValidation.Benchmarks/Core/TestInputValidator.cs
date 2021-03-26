using FairyBread;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
  public class TestInputValidator : AbstractValidator<TestInput>, IRequiresOwnScopeValidator
  {
    public TestInputValidator()
    {
      RuleFor(input => input.Name)
        .NotEmpty()
        .WithMessage("Name is empty");
    }
  }
}
