using System;
using System.Collections.Concurrent;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputValidatorProviderContextExtensions
	{
		private static readonly ConcurrentDictionary<Type, Type> inputFieldTypeToValidatorType = new();

		public static Type GetGenericValidatorType(this InputValidatorProviderContext inputValidatorProviderContext)
		{
			return inputFieldTypeToValidatorType.GetOrAdd(
				inputValidatorProviderContext.InputField.RuntimeType,
				inputFieldType => typeof(IValidator<>).MakeGenericType(inputFieldType));
		}
	}
}
