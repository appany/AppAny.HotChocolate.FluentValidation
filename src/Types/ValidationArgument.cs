using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationArgument
	{
		public Dictionary<string, ValidationPayload[]>? ArgumentErrors { get; set; }
	}
}
