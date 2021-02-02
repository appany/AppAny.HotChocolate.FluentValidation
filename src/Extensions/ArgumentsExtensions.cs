using HotChocolate.Types;
using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ArgumentsExtensions
	{
		public static IInputField? TryGetArgument(this IDictionary<string, IInputField> arguments, string name)
		{
			return arguments.TryGetValue(name, out var data)
				? data
				: null;
		}
	}
}
