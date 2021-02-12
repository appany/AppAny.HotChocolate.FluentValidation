using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationSummaryType : ObjectType<ValidationSummary>
	{
		private readonly string field;
		private readonly Dictionary<string, string[]> arguments;

		public ValidationSummaryType(string field, Dictionary<string, string[]> arguments)
		{
			this.field = field;
			this.arguments = arguments;
		}

		protected override void Configure(IObjectTypeDescriptor<ValidationSummary> descriptor)
		{
			descriptor.Name(field + "Validation");

			foreach (var (argument, properties) in arguments)
			{
				descriptor.Field(argument)
					.Type(new ValidationArgumentType(field, argument, properties))
					.Resolve(context => context.Parent<ValidationSummary>().ValidationErrors?.GetValueOrDefault(argument));
			}
		}
	}
}
