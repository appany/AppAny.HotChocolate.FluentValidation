using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ArgumentExtensions
	{
		public static ArgumentValidationOptions? TryGetArgumentOptions(this IReadOnlyDictionary<string, object?> contextData)
		{
			return contextData.TryGetValue(ValidationDefaults.ArgumentOptionsKey, out var data)
				&& data is ArgumentValidationOptions options
					? options
					: null;
		}
	}
}
