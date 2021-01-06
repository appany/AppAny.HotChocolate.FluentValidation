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

		public async Task Invoke(IMiddlewareContext middlewareContext)
		{
			var inputFields = middlewareContext.Field.Arguments;

			var hasErrors = false;

			if (inputFields is { Count: > 0 })
			{
				var options = middlewareContext.Schema.Services!.GetRequiredService<IOptions<InputValidationOptions>>().Value;

				await foreach (var error in Validate(middlewareContext, inputFields, options).ConfigureAwait(false))
				{
					middlewareContext.ReportError(error);

					hasErrors = true;
				}
			}

			if (hasErrors is false)
			{
				await next(middlewareContext).ConfigureAwait(false);
			}
		}

		private static async IAsyncEnumerable<IError> Validate(
			IMiddlewareContext middlewareContext,
			IEnumerable<IInputField> inputFields,
			InputValidationOptions options)
		{
			foreach (var inputField in inputFields)
			{
				var inputFieldOptions = inputField.TryGetInputFieldOptions();

				if (inputFieldOptions?.SkipValidation is false)
				{
					var errorMappers = inputFieldOptions.ErrorMappers ?? options.ErrorMappers;
					var validatorFactories = inputFieldOptions.ValidatorFactories ?? options.ValidatorFactories;

					await foreach (var validationResult in ValidateInputField(
							middlewareContext, inputField, validatorFactories).ConfigureAwait(false))
					{
						if (validationResult.IsValid is false)
						{
							foreach (var validationFailure in validationResult.Errors)
							{
								var errorBuilder = ErrorBuilder.New();

								foreach (var errorMapper in errorMappers)
								{
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
			IEnumerable<InputValidatorFactory> validatorFactories)
		{
			var argument = middlewareContext.ArgumentValue<object>(inputField.Name);

			foreach (var validatorFactory in validatorFactories)
			{
				var validators = validatorFactory.Invoke(
					new InputValidatorFactoryContext(middlewareContext.Services, inputField.RuntimeType));

				foreach (var validator in validators)
				{
					yield return await validator.ValidateAsync(argument, middlewareContext.RequestAborted);
				}
			}
		}
	}
}
