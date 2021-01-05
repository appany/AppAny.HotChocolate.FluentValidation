using System;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly struct ValidatorFactoryContext
	{
		public ValidatorFactoryContext(IServiceProvider serviceProvider, Type inputFieldType)
		{
			ServiceProvider = serviceProvider;
			InputFieldType = inputFieldType;
		}

		public IServiceProvider ServiceProvider { get; }
		public Type InputFieldType { get; }

		public Type MakeGenericValidatorType()
		{
			return typeof(IValidator<>).MakeGenericType(InputFieldType);
		}
	}
}
