using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputFieldExtensions
	{
		public static FluentValidationInputFieldOptions? TryGetInputFieldOptions(this IInputField inputField)
		{
			return (FluentValidationInputFieldOptions?)inputField
				.ContextData
				.GetValueOrDefault(ValidationDefaults.Keys.InputFieldOptions);
		}
	}
}
