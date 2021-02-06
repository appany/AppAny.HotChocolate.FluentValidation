using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using HotChocolate;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Internal;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidationDefaults
	{
		/// <summary>
		/// Default graphql error code for failed validation
		/// </summary>
		public const string Code = "ValidationFailed";

		/// <summary>
		/// Default <see cref="IHasContextData.ContextData"/> key for <see cref="ValidationOptions"/>
		/// </summary>
		public const string ValidationOptionsKey = "ValidationOptions";

		/// <summary>
		/// Default <see cref="IHasContextData.ContextData"/> key for <see cref="ArgumentValidationOptions"/>
		/// </summary>
		public const string ArgumentOptionsKey = "ArgumentValidationOptions";

		/// <summary>
		/// Default <see cref="IHasContextData.ContextData"/> key for <see cref="ObjectFieldValidationOptions"/>
		/// </summary>
		public const string ObjectFieldOptionsKey = "ObjectFieldValidationOptions";

		/// <summary>
		/// Default validation field middleware
		/// </summary>
		public static FieldMiddleware Middleware { get; } = ValidationFieldMiddleware.Use;

		/// <summary>
		/// Default graphql error extensions keys
		/// </summary>
		public static class ExtensionKeys
		{
			public const string CodeKey = "code";
			public const string ValidatorKey = "validator";
			public const string ArgumentKey = "argument";
			public const string PropertyKey = "property";
			public const string SeverityKey = "severity";
			public const string AttemptedValueKey = "attemptedValue";
			public const string CustomStateKey = "customState";
			public const string FormattedMessagePlaceholderValuesKey = "formattedMessagePlaceholderValues";
		}

		/// <summary>
		/// Default <see cref="FluentValidation.SkipValidation"/> implementations
		/// </summary>
		public static class SkipValidation
		{
			/// <summary>
			/// Default <see cref="SkipValidation"/> implementation. Never skips validation
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ValueTask<bool> Default(SkipValidationContext skipValidationContext)
			{
				return new(false);
			}

			/// <summary>
			/// Always skip <see cref="SkipValidation"/> implementation
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ValueTask<bool> Skip(SkipValidationContext skipValidationContext)
			{
				return new(true);
			}
		}

		/// <summary>
		/// Default <see cref="ErrorMapper"/> implementations
		/// </summary>
		public static class ErrorMappers
		{
			/// <summary>
			/// Maps graphql error code, path and message
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Default(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetCode(Code)
					.SetPath(mappingContext.MiddlewareContext.Path)
					.SetMessage(mappingContext.ValidationFailure.ErrorMessage);
			}

			/// <summary>
			/// Maps useful extensions about input field, property, used validator, invalid value and severity
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Details(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.ValidatorKey, mappingContext.ValidationFailure.ErrorCode)
					.SetExtension(ExtensionKeys.ArgumentKey, mappingContext.Argument.Name)
					.SetExtension(ExtensionKeys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
					.SetExtension(ExtensionKeys.SeverityKey, mappingContext.ValidationFailure.Severity);
			}

			/// <summary>
			/// Maps custom state and formatted message placeholder values
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Extended(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.AttemptedValueKey, mappingContext.ValidationFailure.AttemptedValue)
					.SetExtension(ExtensionKeys.CustomStateKey, mappingContext.ValidationFailure.CustomState)
					.SetExtension(
						ExtensionKeys.FormattedMessagePlaceholderValuesKey,
						mappingContext.ValidationFailure.FormattedMessagePlaceholderValues);
			}
		}

		/// <summary>
		/// Default <see cref="InputValidator"/> implementations
		/// </summary>
		public static class InputValidators
		{
			/// <summary>
			/// Default <see cref="InputValidator"/> implementation
			/// </summary>
			public static async Task<ValidationResult?> Default(InputValidatorContext inputValidatorContext)
			{
				var argumentValue = inputValidatorContext
					.MiddlewareContext
					.ArgumentValue<object?>(inputValidatorContext.Argument.Name);

				if (argumentValue is null)
				{
					return null;
				}

				var validatorType = inputValidatorContext.Argument.GetGenericValidatorType();

				var validators = (IValidator[])inputValidatorContext.MiddlewareContext.Services.GetServices(validatorType);

				var validationContext = new ValidationContext<object>(argumentValue);

				ValidationResult? validationResult = null;

				for (var validatorIndex = 0; validatorIndex < validators.Length; validatorIndex++)
				{
					var validator = validators[validatorIndex];

					var validatorResult = await validator
						.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted)
						.ConfigureAwait(false);

					if (validationResult is null)
					{
						validationResult = validatorResult;
					}
					else
					{
						validationResult.MergeFailures(validatorResult);
					}
				}

				return validationResult;
			}
		}

		/// <summary>
		/// Default <see cref="ValidationStrategy{T}"/> implementations
		/// </summary>
		public static class ValidationStrategies
		{
			/// <summary>
			/// Doing nothing by default.
			/// To override validation strategy use <see cref="ArgumentValidationBuilderExtensions.UseValidator"/>
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Default<TInput>(ValidationStrategy<TInput> validationStrategy)
			{
			}
		}
	}
}
