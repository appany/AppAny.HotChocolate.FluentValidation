using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationResultType<TOutputType> : UnionType
		where TOutputType : ObjectType
	{
		private readonly string field;
		private readonly Dictionary<string, string[]> arguments;

		public ValidationResultType(string field, Dictionary<string, string[]> arguments)
		{
			this.field = field;
			this.arguments = arguments;
		}

		protected override void Configure(IUnionTypeDescriptor descriptor)
		{
			descriptor.Name(field + "Result");

			descriptor.Type<TOutputType>();
			descriptor.Type(new ValidationSummaryType(field, arguments));
		}
	}
}
