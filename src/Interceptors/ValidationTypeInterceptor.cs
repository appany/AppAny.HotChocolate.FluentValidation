using System.Linq;
using System.Collections.Generic;
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
	}
}
