using HotChocolate;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AppAny.HotChocolate.FluentValidation
{
  internal static class ExtensionDataExtensions
  {
    public static void CreateObjectFieldOptions(this ExtensionData extensionData)
    {
      extensionData[ValidationDefaults.ObjectFieldOptionsKey] = new ObjectFieldValidationOptions();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ObjectFieldValidationOptions GetObjectFieldOptions(
      this IReadOnlyDictionary<string, object?> contextData)
    {
      return (ObjectFieldValidationOptions)contextData[ValidationDefaults.ObjectFieldOptionsKey]!;
    }

    public static ObjectFieldValidationOptions? TryGetObjectFieldOptions(
      this IReadOnlyDictionary<string, object?> contextData)
    {
      return contextData.TryGetValue(ValidationDefaults.ObjectFieldOptionsKey, out var data)
        ? (ObjectFieldValidationOptions)data!
        : null;
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArgumentValidationOptions GetArgumentOptions(this IReadOnlyDictionary<string, object?> contextData)
    {
      return (ArgumentValidationOptions)contextData[ValidationDefaults.ArgumentOptionsKey]!;
    }

    public static ArgumentValidationOptions? TryGetArgumentOptions(
      this IReadOnlyDictionary<string, object?> contextData)
    {
      return contextData.TryGetValue(ValidationDefaults.ArgumentOptionsKey, out var data)
        ? (ArgumentValidationOptions)data!
        : null;
    }

    public static bool ShouldValidateArgument(this IReadOnlyDictionary<string, object?> contextData)
    {
      return contextData.TryGetArgumentOptions() is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValidationOptions GetValidationOptions(this IDictionary<string, object?> contextData)
    {
      return (ValidationOptions)contextData[ValidationDefaults.ValidationOptionsKey]!;
    }
  }
}
