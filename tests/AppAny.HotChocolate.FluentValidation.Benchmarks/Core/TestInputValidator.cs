using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public class TestInputValidator : AbstractValidator<TestInput>
	{
		public TestInputValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Name is empty");
		}
	}
}
