using System.Linq;
using HotChocolate;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationSchemaInterceptor : SchemaInterceptor
	{
		public override void OnAfterCreate(IDescriptorContext context, ISchema schema)
		{
			foreach (var objectField in schema.Types.OfType<IObjectType>().SelectMany(type => type.Fields))
			{
				foreach (var argument in objectField.Arguments)
				{
					if (argument.ContextData.ShouldValidate())
					{
						var extensionData = (ExtensionData)objectField.ContextData;

						var objectOptions = extensionData.GetOrCreateObjectOptions();

						objectOptions.Arguments.Add(argument.Name, argument);
					}
				}
			}
		}
	}
}
