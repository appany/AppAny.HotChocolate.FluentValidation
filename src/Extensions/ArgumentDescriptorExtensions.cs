using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ArgumentDescriptorExtensions
	{
		/// <summary>
		/// Configures argument for validation
		/// </summary>
		public static IArgumentDescriptor UseFluentValidation(this IArgumentDescriptor argumentDescriptor)
		{
			return argumentDescriptor.UseFluentValidation(_ =>
			{
			});
		}

		/// <summary>
		/// Configures argument for validation
		/// </summary>
		public static IArgumentDescriptor UseFluentValidation(
			this IArgumentDescriptor argumentDescriptor,
			Action<ArgumentValidationBuilder> configure)
		{
			argumentDescriptor.Extend().OnBeforeCreate(definition =>
			{
				var options = definition.ContextData.GetOrCreateArgumentOptions();

				var configurator = new DefaultArgumentValidationBuilder(options);

				configure.Invoke(configurator);
			});

			return argumentDescriptor;
		}
	}
}
