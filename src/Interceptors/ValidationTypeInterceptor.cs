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
			if (definition is not ObjectTypeDefinition objectTypeDefinition)
			{
				return;
			}

			var validationOptions = completionContext.ContextData.GetValidationOptions();

			foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
			{
				var arguments = objectFieldDefinition.Arguments
					.Where(x => x.ContextData.ShouldValidate())
					.ToList();

				if (arguments is { Count: > 0 })
				{
					foreach (var options in arguments.Select(argument => argument.ContextData.GetArgumentOptions()))
					{
						options.ErrorMappers ??= validationOptions.ErrorMappers;
						options.SkipValidation ??= validationOptions.SkipValidation;
						options.InputValidatorProviders ??= validationOptions.InputValidatorProviders;
					}

					objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationDefaults.Middleware);
				}
			}
		}
	}
}
