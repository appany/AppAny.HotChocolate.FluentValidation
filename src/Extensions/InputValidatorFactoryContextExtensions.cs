using System;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class InputValidatorFactoryContextExtensions
	{
		public static Type MakeGenericValidatorType(this InputValidatorFactoryContext inputValidatorFactoryContext)
		{
			return typeof(IValidator<>).MakeGenericType(inputValidatorFactoryContext.InputFieldType);
		}
	}
}
