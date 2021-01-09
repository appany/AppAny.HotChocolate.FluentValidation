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

				if (inputFields is { Count: > 0 })
				{
					var options = middlewareContext.Schema.Services!.GetRequiredService<IOptions<InputValidationOptions>>().Value;

					for (var inputFieldIndex = 0; inputFieldIndex < inputFields.Count; inputFieldIndex++)
					{
						var inputField = inputFields[inputFieldIndex];

						var inputFieldOptions = inputField.ContextData.TryGetInputFieldOptions();

						var skipValidation = inputFieldOptions?.SkipValidation ?? options.SkipValidation;

						if (skipValidation.Invoke(new SkipValidationContext(middlewareContext, inputField)) is false)
						{
							var errorMappers = inputFieldOptions?.ErrorMappers ?? options.ErrorMappers;
							var validatorFactories = inputFieldOptions?.ValidatorFactories ?? options.ValidatorFactories;

							var argument = middlewareContext.ArgumentValue<object?>(inputField.Name);

							if (argument is null)
							{
								continue;
							}

							for (var validatorFactoryIndex = 0; validatorFactoryIndex < validatorFactories.Count; validatorFactoryIndex++)
							{
								var validatorFactory = validatorFactories[validatorFactoryIndex];

								var validators = validatorFactory.Invoke(
									new InputValidatorFactoryContext(middlewareContext.Services, inputField.RuntimeType));

								foreach (var validator in validators)
								{
									var validationResult = await validator.Invoke(argument, middlewareContext.RequestAborted);

									if (validationResult.IsValid)
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

											errorMapper.Invoke(
												errorBuilder,
												new ErrorMappingContext(middlewareContext, inputField, validationResult, validationFailure));
										}

										middlewareContext.ReportError(errorBuilder.Build());
									}
								}
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
