using System.Collections.Generic;
using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ExtensionDataExtensions
	{
		public static FluentValidationInputFieldOptions GetOrCreateInputFieldOptions(this ExtensionData extensionData)
		{
			var options = extensionData.GetValueOrDefault(ValidationDefaults.Keys.InputFieldOptions);

			if (options is null)
			{
				options = new FluentValidationInputFieldOptions();
				extensionData.Add(ValidationDefaults.Keys.InputFieldOptions, options);
			}

			return (FluentValidationInputFieldOptions)options;
		}
	}
}
