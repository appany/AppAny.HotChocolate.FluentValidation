using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationTypeInterceptor : TypeInterceptor
	{
		public override void OnBeforeCompleteType(
			ITypeCompletionContext completionContext,
			DefinitionBase? definition,
			IDictionary<string, object?> contextData)
		{
			if (definition is ObjectTypeDefinition objectTypeDefinition)
			{
				var validationOptions = completionContext.ContextData.GetValidationOptions();

				foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
				{
					if (objectFieldDefinition.Arguments.Any(argument => argument.ContextData.ShouldValidate()))
					{
						objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationFieldMiddleware.Create(validationOptions));
					}
				}
			}
		}
	}
}
