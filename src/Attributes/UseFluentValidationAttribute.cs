using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Use default validation options.
	/// To override options use Code-first approach <see cref="ArgumentDescriptorExtensions"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class UseFluentValidationAttribute : ArgumentDescriptorAttribute
	{
		public override void OnConfigure(
			IDescriptorContext context,
			IArgumentDescriptor descriptor,
			ParameterInfo parameter)
		{
			var fluentValidationAttributes = parameter.GetCustomAttributes<FluentValidationAttribute>();

			descriptor.UseFluentValidation(options =>
			{
				foreach (var fluentValidationAttribute in fluentValidationAttributes)
				{
					fluentValidationAttribute.Configure(options);
				}
			});
		}
	}
}
