using System;
using System.Collections.Generic;
using AppAny.HotChocolate.FluentValidation.Types;
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

				var validationErrors = new Dictionary<string, ValidationArgument>(StringComparer.InvariantCultureIgnoreCase);

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
						var argumentErrors = new Dictionary<string, ValidationPayload[]>(StringComparer.InvariantCultureIgnoreCase);

						for (var validatorIndex = 0; validatorIndex < inputValidators.Count; validatorIndex++)
						{
							var inputValidator = inputValidators[validatorIndex];

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

								// TODO: Get or create
								argumentErrors[validationFailure.PropertyName] = new ValidationPayload[]
								{
									new()
									{
										Message = validationFailure.ErrorMessage,
										Severity = validationFailure.Severity,
										Validator = validationFailure.ErrorCode
									}
								};
							}
						}

						if (argumentErrors is { Count: > 0 })
						{
							validationErrors.Add(argument.Name, new ValidationArgument
							{
								ArgumentErrors = argumentErrors
							});
						}
					}
				}

				if (validationErrors is { Count: > 0 })
				{
					middlewareContext.Result = new ValidationSummary
					{
						ValidationErrors = validationErrors
					};
				}
				else
				{
					await next(middlewareContext).ConfigureAwait(false);
				}
			};
		}
	}
}
