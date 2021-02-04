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
		private readonly Type[] validators;

		public UseFluentValidationAttribute(params Type[] validators)
		{
			this.validators = validators;
		}

		public override void OnConfigure(
			IDescriptorContext context,
			IArgumentDescriptor descriptor,
			ParameterInfo parameter)
		{
			descriptor.UseFluentValidation(options =>
			{
				foreach (var validator in validators)
				{
					options.UseValidator(validator);
				}
			});
		}
	}
}
