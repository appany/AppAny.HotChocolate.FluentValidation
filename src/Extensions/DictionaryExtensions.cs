using System.Collections.Generic;
using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class DictionaryExtensions
	{
		public static ArgumentValidationOptions GetOrCreateArgumentOptions(this ExtensionData extensionData)
		{
			var options = extensionData.TryGetArgumentOptions();

			if (options is null)
			{
				options = new ArgumentValidationOptions();
				extensionData.Add(ValidationDefaults.ArgumentOptionsKey, options);
			}

			return options;
		}

		public static ArgumentValidationOptions? TryGetArgumentOptions(
			this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetValue(ValidationDefaults.ArgumentOptionsKey, out var data)
				? (ArgumentValidationOptions)data!
				: null;
		}

		public static ArgumentValidationOptions GetArgumentOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return (ArgumentValidationOptions)contextData[ValidationDefaults.ArgumentOptionsKey]!;
		}

		public static bool ShouldValidate(this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetArgumentOptions() is not null;
		}

		public static ArgumentValidationOptions? TryGetArgumentOptions(
			this IDictionary<string, ArgumentValidationOptions> arguments,
			string name)
		{
			return arguments.TryGetValue(name, out var options)
				? options
				: null;
		}

		public static ValidationOptions GetValidationOptions(this IDictionary<string, object?> contextData)
		{
			return (ValidationOptions)contextData[ValidationDefaults.ValidationOptionsKey]!;
		}
	}
}
