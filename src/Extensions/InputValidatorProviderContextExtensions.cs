using System;
using System.Collections.Concurrent;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputValidatorProviderContextExtensions
	{
		private static readonly ConcurrentDictionary<Type, Type> ArgumentTypeToValidatorType = new();

		public static Type GetGenericValidatorType(this InputValidatorProviderContext inputValidatorProviderContext)
		{
			return ArgumentTypeToValidatorType.GetOrAdd(
				inputValidatorProviderContext.Argument.RuntimeType,
				argumentType => typeof(IValidator<>).MakeGenericType(argumentType));
		}
	}
}
