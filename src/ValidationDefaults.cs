using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HotChocolate;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using HotChocolate.Resolvers;
using HotChocolate.Types;
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
			public static void Details(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.ValidatorKey, mappingContext.ValidationFailure.ErrorCode)
					.SetExtension(ExtensionKeys.ArgumentKey, mappingContext.Argument.Name)
					.SetExtension(ExtensionKeys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
					.SetExtension(ExtensionKeys.SeverityKey, mappingContext.ValidationFailure.Severity)
					.SetExtension(ExtensionKeys.AttemptedValueKey, mappingContext.ValidationFailure.AttemptedValue);
			}

			/// <summary>
			/// Maps custom state and formatted message placeholder values
			/// </summary>
			public static void Extended(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
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
			/// Creates <see cref="InputValidator"/> from single <see cref="IValidator"/>
			/// </summary>
			public static InputValidator FromValidator(IValidator validator)
			{
				return async context =>
				{
					var validationContext = new ValidationContext<object>(context.Argument);

					return await validator.ValidateAsync(validationContext, context.CancellationToken).ConfigureAwait(false);
				};
			}

			/// <summary>
			/// Creates <see cref="InputValidator"/> from multiple <see cref="IValidator"/>
			/// </summary>
			public static InputValidator FromValidators(IEnumerable<IValidator> validators)
			{
				return async context =>
				{
					var validationContext = new ValidationContext<object>(context.Argument);

					ValidationResult? validationResult = null;

					foreach (var validator in validators)
					{
						var validatorResult = await validator.ValidateAsync(
							validationContext, context.CancellationToken).ConfigureAwait(false);

						if (validationResult is null)
						{
							validationResult = validatorResult;
						}
						else
						{
							for (var index = 0; index < validatorResult.Errors.Count; index++)
							{
								validationResult.Errors.Add(validatorResult.Errors[index]);
							}
						}
					}

					return validationResult;
				};
			}

			/// <summary>
			/// Creates <see cref="InputValidator"/> from single <see cref="IValidator{TInput}"/> with <see cref="ValidationStrategy{TInput}"/>
			/// </summary>
			public static InputValidator FromValidatorWithStrategy<TInput>(
				IValidator<TInput> validator,
				Action<ValidationStrategy<TInput>> validationStrategy)
			{
				return async context =>
				{
					var validationContext = ValidationContext<TInput>.CreateWithOptions(
						(TInput)context.Argument,
						validationStrategy);

					return await validator.ValidateAsync(validationContext, context.CancellationToken).ConfigureAwait(false);
				};
			}

			/// <summary>
			/// Creates <see cref="InputValidator"/> from multiple <see cref="IValidator{TInput}"/> with <see cref="ValidationStrategy{TInput}"/>
			/// </summary>
			public static InputValidator FromValidatorsWithStrategy<TInput>(
				IEnumerable<IValidator<TInput>> validators,
				Action<ValidationStrategy<TInput>> validationStrategy)
			{
				return async context =>
				{
					var validationContext = ValidationContext<TInput>.CreateWithOptions(
						(TInput)context.Argument,
						validationStrategy);

					ValidationResult? validationResult = null;

					foreach (var validator in validators)
					{
						var validatorResult = await validator.ValidateAsync(
							validationContext, context.CancellationToken).ConfigureAwait(false);

						if (validationResult is null)
						{
							validationResult = validatorResult;
						}
						else
						{
							for (var index = 0; index < validatorResult.Errors.Count; index++)
							{
								validationResult.Errors.Add(validatorResult.Errors[index]);
							}
						}
					}

					return validationResult;
				};
			}
		}

		/// <summary>
		/// Default <see cref="InputValidatorProvider"/> implementations
		/// </summary>
		public static class InputValidatorProviders
		{
			/// <summary>
			/// Resolves all <see cref="IValidator{T}"/> implementations from <see cref="IHasRuntimeType.RuntimeType"/>
			/// </summary>
			public static InputValidator Default(InputValidatorProviderContext inputValidatorProviderContext)
			{
				var validatorType = inputValidatorProviderContext.Argument.GetGenericValidatorType();

				var validators = (IEnumerable<IValidator>)inputValidatorProviderContext
					.MiddlewareContext
					.Services
					.GetServices(validatorType);

				return InputValidators.FromValidators(validators);
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
