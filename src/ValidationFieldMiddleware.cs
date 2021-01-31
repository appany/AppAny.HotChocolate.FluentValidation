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

						var argumentValue = middlewareContext.ArgumentValue<object?>(argument.Name);

						if (argumentValue is null)
						{
							continue;
						}

						for (var providerIndex = 0; providerIndex < argumentOptions.InputValidatorProviders!.Count; providerIndex++)
						{
							var inputValidatorProvider = argumentOptions.InputValidatorProviders[providerIndex];

							var inputValidator = inputValidatorProvider
								.Invoke(new InputValidatorProviderContext(middlewareContext, argument));

							var validationResult = await inputValidator
								.Invoke(new InputValidatorContext(argumentValue, middlewareContext.RequestAborted))
								.ConfigureAwait(false);

							if (validationResult?.IsValid is null or true)
							{
								continue;
							}

							for (var errorIndex = 0; errorIndex < validationResult.Errors.Count; errorIndex++)
							{
								var validationFailure = validationResult.Errors[errorIndex];

								var errorBuilder = ErrorBuilder.New();

								argumentOptions.ErrorMapper!
									.Invoke(
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
