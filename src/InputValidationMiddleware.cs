using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class InputValidationMiddleware
	{
		private readonly FieldDelegate next;

		public InputValidationMiddleware(FieldDelegate next)
		{
			this.next = next;
		}

		public async ValueTask Invoke(IMiddlewareContext middlewareContext)
		{
			var inputFields = middlewareContext.Field.Arguments;

			if (inputFields is { Count: > 0 })
			{
				var options = middlewareContext.Schema.Services!.GetRequiredService<IOptions<InputValidationOptions>>().Value;

				await foreach (var error in Validate(middlewareContext, inputFields, options)
					.WithCancellation(middlewareContext.RequestAborted)
					.ConfigureAwait(false))
				{
					middlewareContext.ReportError(error);
				}
			}

			if (middlewareContext.HasErrors is false)
			{
				await next(middlewareContext).ConfigureAwait(false);
			}
		}

		private static async IAsyncEnumerable<IError> Validate(
			IMiddlewareContext middlewareContext,
			IFieldCollection<IInputField> inputFields,
			InputValidationOptions options)
		{
			for (var inputFieldIndex = 0; inputFieldIndex < inputFields.Count; inputFieldIndex++)
			{
				var inputField = inputFields[inputFieldIndex];

				var inputFieldOptions = inputField.TryGetInputFieldOptions();

				var skipValidation = inputFieldOptions?.SkipValidation ?? options.SkipValidation;

				if (skipValidation.Invoke(new SkipValidationContext(middlewareContext, inputField)) is false)
				{
					var errorMappers = inputFieldOptions?.ErrorMappers ?? options.ErrorMappers;
					var validatorFactories = inputFieldOptions?.ValidatorFactories ?? options.ValidatorFactories;

					await foreach (var validationResult in ValidateInputField(middlewareContext, inputField, validatorFactories)
						.WithCancellation(middlewareContext.RequestAborted)
						.ConfigureAwait(false))
					{
						if (validationResult.IsValid is false)
						{
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

								yield return errorBuilder.Build();
							}
						}
					}
				}
			}
		}

		private static async IAsyncEnumerable<ValidationResult> ValidateInputField(
			IMiddlewareContext middlewareContext,
			IInputField inputField,
			IList<InputValidatorFactory> validatorFactories)
		{
			var argument = middlewareContext.ArgumentValue<object?>(inputField.Name);

			if (argument is null)
			{
				yield break;
			}

			for (var validatorFactoryIndex = 0; validatorFactoryIndex < validatorFactories.Count; validatorFactoryIndex++)
			{
				var validatorFactory = validatorFactories[validatorFactoryIndex];

				var validators = validatorFactory.Invoke(
					new InputValidatorFactoryContext(middlewareContext.Services, inputField.RuntimeType));

				foreach (var validator in validators)
				{
					yield return await validator.Invoke(argument, middlewareContext.RequestAborted);
				}
			}
		}
	}
}
