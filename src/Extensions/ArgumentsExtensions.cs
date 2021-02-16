using HotChocolate.Types;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ArgumentsExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IInputField? TryGetArgument(this IDictionary<string, IInputField> arguments, string name)
		{
			return arguments.TryGetValue(name, out var data)
				? data
				: null;
		}
	}
}
