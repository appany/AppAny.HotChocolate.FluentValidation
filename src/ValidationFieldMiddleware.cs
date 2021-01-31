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
				var passedArguments = middlewareContext.Selection.SyntaxNode.Arguments;

				if (passedArguments is { Count: > 0 })
				{
					var objectFieldOptions = middlewareContext.Field.ContextData.GetObjectFieldOptions();

					for (var passedArgumentIndex = 0; passedArgumentIndex < passedArguments.Count; passedArgumentIndex++)
					{
						var passedArgument = passedArguments[passedArgumentIndex];

						var argument = objectFieldOptions.Arguments.TryGetArgument(passedArgument.Name.Value);

						if (argument is null)
						{
							continue;
						}

						var argumentOptions = argument.ContextData.GetArgumentOptions();

						if (await argumentOptions.SkipValidation!.Invoke(
							new SkipValidationContext(middlewareContext, argument)).ConfigureAwait(false))
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

							var inputValidator = inputValidatorProvider.Invoke(
								new InputValidatorProviderContext(middlewareContext, argument));

							var validationResult = await inputValidator.Invoke(
								argumentValue, middlewareContext.RequestAborted).ConfigureAwait(false);

							if (validationResult?.IsValid is null or true)
							{
								continue;
							}

							for (var errorIndex = 0; errorIndex < validationResult.Errors.Count; errorIndex++)
							{
								var validationFailure = validationResult.Errors[errorIndex];

								var errorBuilder = ErrorBuilder.New();

								for (var errorMapperIndex = 0; errorMapperIndex < argumentOptions.ErrorMappers!.Count; errorMapperIndex++)
								{
									var errorMapper = argumentOptions.ErrorMappers[errorMapperIndex];

									errorMapper.Invoke(errorBuilder, new ErrorMappingContext(
										middlewareContext,
										argument,
										validationResult,
										validationFailure));
								}

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
