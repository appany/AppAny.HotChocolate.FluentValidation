using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationSummary
	{
		public Dictionary<string, ValidationArgument>? ValidationErrors { get; set; }
	}
}
