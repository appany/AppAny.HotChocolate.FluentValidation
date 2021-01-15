using System;
using System.Collections.Concurrent;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputValidatorFactoryContextExtensions
	{
		private static readonly ConcurrentDictionary<Type, Type> inputFieldTypeToValidatorType = new();

		public static Type GetGenericValidatorType(this InputValidatorFactoryContext inputValidatorFactoryContext)
		{
			return inputFieldTypeToValidatorType.GetOrAdd(
				inputValidatorFactoryContext.InputField.RuntimeType,
				inputFieldType => typeof(IValidator<>).MakeGenericType(inputFieldType));
		}
	}
}
