using System;
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
					.ToArray();

				if (argumentOptions is { Length: > 0 })
				{
					foreach (var options in argumentOptions)
					{
						options.MergeValidationOptions(validationOptions);
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

		public static void OnBeforeRegisterDependencies(
			ITypeDiscoveryContext discoveryContext,
			DefinitionBase? definition,
			IDictionary<string, object?> contextdata)
		{
			if (definition is not ObjectTypeDefinition objectTypeDefinition)
			{
				return;
			}

			foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
			{
				var argumentNames = objectFieldDefinition.Arguments
					.Where(argument => argument.ContextData.ShouldValidateArgument())
					.Select(argument => argument.Name.Value)
					.ToArray();

				if (argumentNames is { Length: > 0 })
				{
					var innerType = objectFieldDefinition.Type switch
					{
						ExtendedTypeReference extendedTypeReference => extendedTypeReference.Type.Type,
						SchemaTypeReference schemaTypeReference => schemaTypeReference.Type.GetType(),
						_ => throw new InvalidOperationException()
					};

					objectFieldDefinition.ContextData.Add("TEst", "Works!");

					var type = typeof(ValidationResultType<>).MakeGenericType(innerType);

					var fieldName = objectFieldDefinition.Name.Value;

					var typeName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

					var typeInstance = type.GetConstructors()
						.Single()
						.Invoke(new object[]{ typeName, argumentNames });

					objectFieldDefinition.Type = new SchemaTypeReference((IOutputType)typeInstance);

					objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationDefaults.Middleware);
				}
			}
		}
	}
}
