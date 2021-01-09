using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputFieldExtensions
	{
		public static InputFieldValidationOptions? TryGetInputFieldOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetValue(ValidationDefaults.InputFieldOptionsKey, out var data)
				&& data is InputFieldValidationOptions options
					? options
					: null;
		}
	}
}
