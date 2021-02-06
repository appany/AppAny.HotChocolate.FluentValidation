using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ValidationInterceptors
	{
		public static void OnBeforeCompleteType(
			ITypeCompletionContext completionContext,
			DefinitionBase? definition,
			IDictionary<string, object?> contextData)
		{
			if (definition is not ObjectTypeDefinition objectTypeDefinition)
			{
				return;
			}

			var validationOptions = completionContext.ContextData.GetValidationOptions();

			foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
			{
				var argumentOptions = objectFieldDefinition.Arguments
					.Where(argument => argument.ContextData.ShouldValidateArgument())
					.Select(argument => argument.ContextData.GetArgumentOptions())
					.ToList();

				if (argumentOptions is { Count: > 0 })
				{
					foreach (var options in argumentOptions)
					{
						options.ErrorMapper ??= validationOptions.ErrorMapper;
						options.SkipValidation ??= validationOptions.SkipValidation;
						options.InputValidators ??= validationOptions.InputValidators;
					}

					objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationDefaults.Middleware);
				}
			}
		}

		public static void OnAfterCreate(IDescriptorContext context, ISchema schema)
		{
			foreach (var objectField in schema.Types.OfType<IObjectType>().SelectMany(type => type.Fields))
			{
				foreach (var argument in objectField.Arguments.Where(arg => arg.ContextData.ShouldValidateArgument()))
				{
					var extensionData = (ExtensionData)objectField.ContextData;

					var objectOptions = extensionData.GetOrCreateObjectFieldOptions();

					objectOptions.Arguments.Add(argument.Name, argument);
				}
			}
		}
	}
}
