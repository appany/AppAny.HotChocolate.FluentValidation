using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace AppAny.HotChocolate.FluentValidation
{
	internal readonly struct ValidationFieldMiddlewareContext
	{
		public ValidationFieldMiddlewareContext(
			ValidationOptions validationOptions,
			IDictionary<string, ArgumentValidationOptions> argumentOptions)
		{
			ValidationOptions = validationOptions;
			ArgumentOptions = argumentOptions;
		}

		public ValidationOptions ValidationOptions { get; }
		public IDictionary<string, ArgumentValidationOptions> ArgumentOptions { get; }
	}

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
					var argumentOptions = objectFieldDefinition.Arguments
						.Where(x => x.ContextData.ShouldValidate())
						.Select(x => new KeyValuePair<string, ArgumentValidationOptions>(x.Name, x.ContextData.GetArgumentOptions()))
						.ToArray();

					if (argumentOptions.Any())
					{
						objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationFieldMiddleware.Create(
							new ValidationFieldMiddlewareContext(
								validationOptions,
								new ConcurrentDictionary<string, ArgumentValidationOptions>(argumentOptions))));
					}
				}
			}
		}
	}
}
