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
				var options = objectFieldDefinition.Arguments
					.Where(argument => argument.ContextData.ShouldValidate())
					.Select(argument => argument.ContextData.GetArgumentOptions())
					.ToList();

				if (options is { Count: > 0 })
				{
					foreach (var option in options)
					{
						option.ErrorMapper ??= validationOptions.ErrorMapper;
						option.SkipValidation ??= validationOptions.SkipValidation;
						option.InputValidatorProviders ??= validationOptions.InputValidatorProviders;
					}

					objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationDefaults.Middleware);
				}
			}
		}
	}
}
