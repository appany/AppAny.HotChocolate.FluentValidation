using System.Collections.Generic;
using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ExtensionDataExtensions
	{
		public static InputFieldValidationOptions GetOrCreateInputFieldOptions(this ExtensionData extensionData)
		{
			var options = extensionData.GetValueOrDefault(ValidationDefaults.InputFieldOptionsKey);

			if (options is null)
			{
				options = new InputFieldValidationOptions();
				extensionData.Add(ValidationDefaults.InputFieldOptionsKey, options);
			}

			return (InputFieldValidationOptions)options;
		}
	}
}
