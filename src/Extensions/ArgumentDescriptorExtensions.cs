using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ArgumentDescriptorExtensions
	{
		public static IArgumentDescriptor UseFluentValidation(this IArgumentDescriptor argumentDescriptor)
		{
			return argumentDescriptor.UseFluentValidation(_ =>
			{
			});
		}

		public static IArgumentDescriptor UseFluentValidation(
			this IArgumentDescriptor argumentDescriptor,
			Action<IValidationInputFieldConfigurator> configure)
		{
			argumentDescriptor.Extend().OnBeforeCreate(definition =>
			{
				var options = definition.ContextData.GetOrCreateInputFieldOptions();

				var configurator = new ValidationInputFieldConfigurator(options);

				configure.Invoke(configurator);
			});

			return argumentDescriptor;
		}
	}
}
