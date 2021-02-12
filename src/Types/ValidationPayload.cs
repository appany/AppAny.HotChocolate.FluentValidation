using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationPayload
	{
		public string Message { get; set; } = default!;
		public string Validator { get; set; } = default!;
		public Severity Severity { get; set; } = default!;
	}
}
