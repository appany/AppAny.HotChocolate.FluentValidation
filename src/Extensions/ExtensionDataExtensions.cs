using System.Collections.Generic;
using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ExtensionDataExtensions
	{
		public static ValidationInputFieldOptions GetOrCreateInputFieldOptions(this ExtensionData extensionData)
		{
			var options = extensionData.GetValueOrDefault(ValidationDefaults.InputFieldOptions);

			if (options is null)
			{
				options = new ValidationInputFieldOptions();
				extensionData.Add(ValidationDefaults.InputFieldOptions, options);
			}

			return (ValidationInputFieldOptions)options;
		}
	}
}
