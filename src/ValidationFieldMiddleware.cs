using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidationFieldMiddleware
	{
		public static FieldDelegate Use(FieldDelegate next)
		{
			return async middlewareContext =>
			{
				var passedArguments = middlewareContext.Selection.SyntaxNode.Arguments;

				if (passedArguments is { Count: 0 })
				{
					await next(middlewareContext).ConfigureAwait(false);
					return;
				}

				var objectOptions = middlewareContext.Field.ContextData.TryGetObjectOptions();

				if (objectOptions is null)
				{
					await next(middlewareContext).ConfigureAwait(false);
					return;
				}

				var validationOptions = middlewareContext.Schema.Services!
					.GetRequiredService<IOptions<ValidationOptions>>()
					.Value;

				for (var passedArgumentIndex = 0; passedArgumentIndex < passedArguments.Count; passedArgumentIndex++)
				{
					var passedArgument = passedArguments[passedArgumentIndex];

					var argument = objectOptions.Arguments.TryGetArgument(passedArgument.Name.Value);

					if (argument is null)
					{
						continue;
					}

					var argumentOptions = argument.ContextData.GetArgumentOptions();

					var skipValidation = argumentOptions.SkipValidation ?? validationOptions.SkipValidation;

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
						?? validationOptions.ErrorMappers;

					var inputValidatorProviders = argumentOptions.InputValidatorProviders
						?? validationOptions.InputValidatorProviders;

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

				if (middlewareContext.HasErrors is false)
				{
					await next(middlewareContext).ConfigureAwait(false);
				}
			};
		}
	}
}
