using System;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly struct InputValidatorFactoryContext
	{
		public InputValidatorFactoryContext(IServiceProvider serviceProvider, Type inputFieldType)
		{
			ServiceProvider = serviceProvider;
			InputFieldType = inputFieldType;
		}

		public IServiceProvider ServiceProvider { get; }
		public Type InputFieldType { get; }
	}
}
