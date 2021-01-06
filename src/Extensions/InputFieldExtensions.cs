using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputFieldExtensions
	{
		public static ValidationInputFieldOptions? TryGetInputFieldOptions(this IInputField inputField)
		{
			return (ValidationInputFieldOptions?)inputField
				.ContextData
				.GetValueOrDefault(ValidationDefaults.InputFieldOptions);
		}
	}
}
