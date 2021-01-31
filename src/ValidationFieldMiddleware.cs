using HotChocolate;
using HotChocolate.Resolvers;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ValidationFieldMiddleware
	{
		public static FieldMiddleware Create(ValidationFieldMiddlewareContext validationContext)
		{
			return next => async middlewareContext =>
			{
				var passedArguments = middlewareContext.Selection.SyntaxNode.Arguments;

				if (passedArguments is { Count: > 0 })
				{
					var arguments = middlewareContext.Field.Arguments;

					for (var argumentIndex = 0; argumentIndex < arguments.Count; argumentIndex++)
					{
						var argument = arguments[argumentIndex];

						var argumentOptions = validationContext.ArgumentOptions.TryGetArgumentOptions(argument.Name);

						if (argumentOptions is null)
						{
							continue;
						}

						var skipValidation = argumentOptions.SkipValidation ?? validationContext.ValidationOptions.SkipValidation;

						if (await skipValidation.Invoke(
							new SkipValidationContext(middlewareContext, argument)).ConfigureAwait(false))
						{
							continue;
						}

						var argumentValue = middlewareContext.ArgumentValue<object?>(argument.Name);

						if (argumentValue is null)
						{
							continue;
						}

						var errorMappers = argumentOptions.ErrorMappers
							?? validationContext.ValidationOptions.ErrorMappers;

						var inputValidatorProviders = argumentOptions.InputValidatorProviders
							?? validationContext.ValidationOptions.InputValidatorProviders;

						for (var providerIndex = 0; providerIndex < inputValidatorProviders.Count; providerIndex++)
						{
							var inputValidatorProvider = inputValidatorProviders[providerIndex];

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

								for (var errorMapperIndex = 0; errorMapperIndex < errorMappers.Count; errorMapperIndex++)
								{
									var errorMapper = errorMappers[errorMapperIndex];

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
