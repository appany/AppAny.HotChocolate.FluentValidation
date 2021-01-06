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
		public const string Code = "ValidationFailed";

		public const string InputFieldOptions = "ValidationInputFieldOptions";

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

		public static class ErrorMappers
		{
			public static void Default(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetCode(Code)
					.SetPath(mappingContext.MiddlewareContext.Path)
					.SetMessage(mappingContext.ValidationFailure.ErrorMessage);
			}

			public static void Details(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.ValidatorKey, mappingContext.ValidationFailure.ErrorCode)
					.SetExtension(ExtensionKeys.InputFieldKey, mappingContext.InputField.Name)
					.SetExtension(ExtensionKeys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
					.SetExtension(ExtensionKeys.SeverityKey, mappingContext.ValidationFailure.Severity)
					.SetExtension(ExtensionKeys.AttemptedValueKey, mappingContext.ValidationFailure.AttemptedValue);
			}

			public static void Extended(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(ExtensionKeys.CustomStateKey, mappingContext.ValidationFailure.CustomState)
					.SetExtension(
						ExtensionKeys.FormattedMessagePlaceholderValuesKey,
						mappingContext.ValidationFailure.FormattedMessagePlaceholderValues);
			}
		}

		public static class ValidatorFactories
		{
			public static IEnumerable<IInputValidator> Default(ValidatorFactoryContext validatorFactoryContext)
			{
				var validatorType = validatorFactoryContext.MakeGenericValidatorType();

				return validatorFactoryContext.ServiceProvider.GetServices(validatorType)
					.OfType<IValidator>()
					.Select(validator => IInputValidator.FromValidator(validator));
			}
		}

		public static class ValidationStrategies
		{
			public static void Default<TInput>(ValidationStrategy<TInput> validationStrategy)
			{
			}
		}
	}
}
