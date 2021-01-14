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
				var inputFields = middlewareContext.Field.Arguments;
				var selectionArguments = middlewareContext.Selection.SyntaxNode.Arguments;

				if (inputFields is { Count: > 0 } && selectionArguments is { Count: > 0 })
				{
					var options = middlewareContext.Schema.Services!
						.GetRequiredService<IOptionsSnapshot<InputValidationOptions>>().Value;

					for (var inputFieldIndex = 0; inputFieldIndex < inputFields.Count; inputFieldIndex++)
					{
						var inputField = inputFields[inputFieldIndex];

						var inputFieldOptions = inputField.ContextData.TryGetInputFieldOptions();

						if (inputFieldOptions is null)
						{
							continue;
						}

						var skipValidation = inputFieldOptions.SkipValidation ?? options.SkipValidation;

						if (await skipValidation.Invoke(new SkipValidationContext(middlewareContext, inputField)))
						{
							continue;
						}

						var argument = middlewareContext.ArgumentValue<object?>(inputField.Name);

						if (argument is null)
						{
							continue;
						}

						var errorMappers = inputFieldOptions.ErrorMappers ?? options.ErrorMappers;
						var inputValidatorFactories = inputFieldOptions.InputValidatorFactories ?? options.InputValidatorFactories;

						for (var inputValidatorFactoryIndex = 0;
							inputValidatorFactoryIndex < inputValidatorFactories.Count;
							inputValidatorFactoryIndex++)
						{
							var inputValidatorFactory = inputValidatorFactories[inputValidatorFactoryIndex];

							var inputValidator = inputValidatorFactory.Invoke(new InputValidatorFactoryContext(
								middlewareContext,
								inputField.RuntimeType));

							var validationResult = await inputValidator.Invoke(argument, middlewareContext.RequestAborted);

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
										inputField,
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
