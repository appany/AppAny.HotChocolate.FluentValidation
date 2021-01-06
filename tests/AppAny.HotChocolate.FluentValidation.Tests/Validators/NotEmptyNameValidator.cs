using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class NotEmptyNameValidator : AbstractValidator<TestPersonInput>
	{
		public const string Message = "Name is empty";

		public NotEmptyNameValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(Message);
		}
	}
}
