using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputFieldExtensions
	{
		public static InputFieldValidationOptions? TryGetInputFieldOptions(this IInputField inputField)
		{
			return (InputFieldValidationOptions?)inputField
				.ContextData
				.GetValueOrDefault(ValidationDefaults.InputFieldOptions);
		}
	}
}
