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
			foreach (var objectType in schema.Types.OfType<IObjectType>())
			{
				foreach (var objectField in objectType.Fields)
				{
					foreach (var argument in objectField.Arguments)
					{
						var options = argument.ContextData.TryGetArgumentOptions();

						if (options is not null)
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
}
