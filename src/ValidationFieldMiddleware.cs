using HotChocolate;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ValidationFieldMiddleware
	{
		public static FieldDelegate Use(FieldDelegate next)
		{
			return async middlewareContext =>
			{
				var argumentNodes = middlewareContext.Selection.SyntaxNode.Arguments;

				if (argumentNodes is { Count: > 0 })
				{
					var objectFieldOptions = middlewareContext.Field.ContextData.GetObjectFieldOptions();

					for (var nodeIndex = 0; nodeIndex < argumentNodes.Count; nodeIndex++)
					{
						var argumentNode = argumentNodes[nodeIndex];

						var argument = objectFieldOptions.Arguments.TryGetArgument(argumentNode.Name.Value);

						if (argument is null)
						{
							continue;
						}

						var argumentOptions = argument.ContextData.GetArgumentOptions();

						var shouldSkipValidation = await argumentOptions.SkipValidation!
							.Invoke(new SkipValidationContext(middlewareContext, argument))
							.ConfigureAwait(false);

						if (shouldSkipValidation)
						{
							continue;
						}

						var inputValidators = argumentOptions.InputValidators!;

						for (var providerIndex = 0; providerIndex < inputValidators.Count; providerIndex++)
						{
							var inputValidator = inputValidators[providerIndex];

							var validationResult = await inputValidator
								.Invoke(new InputValidatorContext(middlewareContext, argument))
								.ConfigureAwait(false);

							if (validationResult?.IsValid is null or true)
							{
								continue;
							}

							for (var errorIndex = 0; errorIndex < validationResult.Errors.Count; errorIndex++)
							{
								var validationFailure = validationResult.Errors[errorIndex];

								var errorBuilder = ErrorBuilder.New();

								argumentOptions.ErrorMapper!.Invoke(
									errorBuilder,
									new ErrorMappingContext(middlewareContext, argument, validationResult, validationFailure));

								middlewareContext.ReportError(errorBuilder.Build());
							}
						}
					}
				}

				if (middlewareContext.HasErrors is false)
				{
					await next(middlewareContext).ConfigureAwait(false);
				}
			};
		}
	}
}
