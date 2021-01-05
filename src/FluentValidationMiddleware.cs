using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	internal class FluentValidationMiddleware
	{
		private readonly FieldDelegate next;

		public FluentValidationMiddleware(FieldDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(IMiddlewareContext middlewareContext)
		{
			var inputFields = middlewareContext.Field.Arguments;

			var hasErrors = false;

			if (inputFields is { Count: > 0 })
			{
				var options = middlewareContext.Schema.Services!.GetRequiredService<IOptions<FluentValidationOptions>>().Value;

				await foreach (var error in Validate(middlewareContext, inputFields, options))
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
			FluentValidationOptions options)
		{
			foreach (var inputField in inputFields)
			{
				var inputFieldOptions = inputField.TryGetInputFieldOptions();

				if (inputFieldOptions?.SkipValidation is false)
				{
					var errorMappers = inputFieldOptions.ErrorMappers ?? options.ErrorMappers;
					var validatorFactories = inputFieldOptions.ValidatorFactories ?? options.ValidatorFactories;

					await foreach (var validationResult in ValidateArguments(middlewareContext, inputField, validatorFactories))
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

		private static async IAsyncEnumerable<ValidationResult> ValidateArguments(
			IMiddlewareContext middlewareContext,
			IInputField inputField,
			IEnumerable<ValidatorFactory> validatorsFactories)
		{
			foreach (var validatorFactory in validatorsFactories)
			{
				var validators = validatorFactory.Invoke(
					new ValidatorFactoryContext(middlewareContext.Services, inputField.RuntimeType));

				foreach (var validator in validators)
				{
					var validationContext = new ValidationContext<object>(
						middlewareContext.ArgumentValue<object>(inputField.Name));

					yield return await validator.ValidateAsync(validationContext, middlewareContext.RequestAborted);
				}
			}
		}
	}
}
