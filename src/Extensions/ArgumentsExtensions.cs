using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HotChocolate.Types;

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
