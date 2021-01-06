using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using FluentValidation;
using FluentValidation.Internal;
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
		/// Default <see cref="IHasContextData.ContextData"/> key for <see cref="InputFieldValidationOptions"/>
		/// </summary>
		public const string InputFieldOptionsKey = "ValidationInputFieldOptions";

		/// <summary>
		/// Default graphql error extensions keys
		/// </summary>
		public static class ExtensionKeys
		{
			public const string CodeKey = "code";
			public const string ValidatorKey = "validator";
			public const string InputFieldKey = "inputField";
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
			public static bool Default(SkipValidationContext skipValidationContext)
			{
				return false;
			}

			/// <summary>
			/// Always skip <see cref="SkipValidation"/> implementation
			/// </summary>
			public static bool Skip(SkipValidationContext skipValidationContext)
			{
				return true;
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
			public static void Default(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetCode(Code)
					.SetPath(mappingContext.MiddlewareContext.Path)
					.SetMessage(mappingContext.ValidationFailure.ErrorMessage);
			}

			/// <summary>
			/// Maps useful extensions about input field, property, used validator, invalid value and severity
			/// </summary>
			public static void Details(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.ValidatorKey, mappingContext.ValidationFailure.ErrorCode)
					.SetExtension(ExtensionKeys.InputFieldKey, mappingContext.InputField.Name)
					.SetExtension(ExtensionKeys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
					.SetExtension(ExtensionKeys.SeverityKey, mappingContext.ValidationFailure.Severity)
					.SetExtension(ExtensionKeys.AttemptedValueKey, mappingContext.ValidationFailure.AttemptedValue);
			}

			/// <summary>
			/// Maps custom state and formatted message placeholder values
			/// </summary>
			public static void Extended(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.CustomStateKey, mappingContext.ValidationFailure.CustomState)
					.SetExtension(
						ExtensionKeys.FormattedMessagePlaceholderValuesKey,
						mappingContext.ValidationFailure.FormattedMessagePlaceholderValues);
			}
		}

		/// <summary>
		/// Default <see cref="InputValidatorFactory"/> implementations
		/// </summary>
		public static class ValidatorFactories
		{
			/// <summary>
			/// Resolves all <see cref="IValidator{T}"/> implementations
			/// </summary>
			public static IEnumerable<IInputValidator> Default(InputValidatorFactoryContext inputValidatorFactoryContext)
			{
				var validatorType = inputValidatorFactoryContext.MakeGenericValidatorType();

				return inputValidatorFactoryContext.ServiceProvider.GetServices(validatorType)
					.OfType<IValidator>()
					.Select(validator => IInputValidator.FromValidator(validator));
			}
		}

		/// <summary>
		/// Default <see cref="ValidationStrategy{T}"/> implementations
		/// </summary>
		public static class ValidationStrategies
		{
			/// <summary>
			/// Doing nothing by default.
			/// To override validation strategy use <see cref="InputFieldValidationConfiguratorExtensions.UseValidator{TValidator}"/> on <see cref="IInputFieldValidationConfigurator"/>
			/// </summary>
			public static void Default<TInput>(ValidationStrategy<TInput> validationStrategy)
			{
			}
		}
	}
}
