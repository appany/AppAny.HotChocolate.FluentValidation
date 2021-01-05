using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class NotEmptyNameValidator : AbstractValidator<TestInput>
	{
		public const string Message = "Name is empty";

		public NotEmptyNameValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(Message);
		}
	}

	public class DoubleNotEmptyNameValidator : AbstractValidator<TestInput>
	{
		public const string Message1 = "Name is empty1";
		public const string Message2 = "Name is empty2";

		public DoubleNotEmptyNameValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(Message1);

			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage(Message2);
		}
	}
}
