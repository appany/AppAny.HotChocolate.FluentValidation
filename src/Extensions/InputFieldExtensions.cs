using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using FluentValidation;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
  internal static class InputFieldExtensions
  {
    private static readonly ConcurrentDictionary<Type, Type> ArgumentTypeToValidatorType = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetGenericValidatorType(this IInputField inputField)
    {
      return ArgumentTypeToValidatorType.GetOrAdd(
        inputField.RuntimeType,
        argumentType => typeof(IValidator<>).MakeGenericType(argumentType));
    }
  }
}
