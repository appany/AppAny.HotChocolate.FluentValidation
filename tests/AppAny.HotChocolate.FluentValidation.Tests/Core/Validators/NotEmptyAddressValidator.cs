using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class NotEmptyAddressValidator : AbstractValidator<TestPersonInput>
	{
		public const string Message = "Address is empty";

		public NotEmptyAddressValidator()
		{
			RuleFor(input => input.Address)
				.NotEmpty()
				.WithMessage(Message);
		}
	}
}
