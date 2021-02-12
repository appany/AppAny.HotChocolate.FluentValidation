using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationArgumentType : ObjectType<ValidationArgument>
	{
		private readonly string field;
		private readonly string argument;
		private readonly string[] properties;

		public ValidationArgumentType(string field, string argument, string[] properties)
		{
			this.field = field;
			this.argument = argument;
			this.properties = properties;
		}

		protected override void Configure(IObjectTypeDescriptor<ValidationArgument> descriptor)
		{
			descriptor.Name(field + char.ToUpper(argument[0]) + argument[1..] + "Argument");

			foreach (var property in properties)
			{
				descriptor.Field(property)
					.Type<ListType<ValidationPayloadType>>()
					.Resolve(context => context.Parent<ValidationArgument>().ArgumentErrors?.GetValueOrDefault(property));
			}
		}
	}
}
