using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

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

		public static ObjectValidationOptions GetOrCreateObjectOptions(this ExtensionData extensionData)
		{
			var options = extensionData.TryGetObjectOptions();

			if (options is null)
			{
				options = new ObjectValidationOptions();
				extensionData.Add(ValidationDefaults.ObjectOptionsKey, options);
			}

			return options;
		}

		public static ObjectValidationOptions? TryGetObjectOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetValue(ValidationDefaults.ObjectOptionsKey, out var data)
				? (ObjectValidationOptions)data!
				: null;
		}

		public static ObjectValidationOptions GetObjectOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return (ObjectValidationOptions)contextData[ValidationDefaults.ObjectOptionsKey]!;
		}

		public static IInputField? TryGetArgument(this IDictionary<string, IInputField> arguments, string name)
		{
			return arguments.TryGetValue(name, out var data)
				? data
				: null;
		}
	}
}
