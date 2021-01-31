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

		public static ObjectFieldValidationOptions GetOrCreateObjectFieldOptions(this ExtensionData extensionData)
		{
			var options = extensionData.TryGetObjectFieldOptions();

			if (options is null)
			{
				options = new ObjectFieldValidationOptions();
				extensionData.Add(ValidationDefaults.ObjectFieldOptionsKey, options);
			}

			return options;
		}

		public static ObjectFieldValidationOptions? TryGetObjectFieldOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetValue(ValidationDefaults.ObjectFieldOptionsKey, out var data)
				? (ObjectFieldValidationOptions)data!
				: null;
		}

		public static ObjectFieldValidationOptions GetObjectFieldOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return (ObjectFieldValidationOptions)contextData[ValidationDefaults.ObjectFieldOptionsKey]!;
		}

		public static IInputField? TryGetArgument(this IDictionary<string, IInputField> arguments, string name)
		{
			return arguments.TryGetValue(name, out var data)
				? data
				: null;
		}

		public static ValidationOptions GetValidationOptions(this IDictionary<string, object?> contextData)
		{
			return (ValidationOptions)contextData[ValidationDefaults.ValidationOptionsKey]!;
		}
	}
}
