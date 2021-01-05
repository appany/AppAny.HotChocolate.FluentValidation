using System.Collections.Generic;
using HotChocolate;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class ValidationDefaults
	{
		public const string ErrorCode = "VALIDATION_ERROR";

		public static class Keys
		{
			public const string InputFieldOptions = "FLUENT_VALIDATION_INPUT_FIELD_OPTIONS";

			public const string ErrorCodeKey = "code";
			public const string ValidatorKey = "validator";
			public const string InputFieldKey = "inputField";
			public const string PropertyKey = "property";
			public const string SeverityKey = "severity";
			public const string AttemptedValueKey = "attemptedValue";
		}

		public static class ErrorMappers
		{
			public static void Default(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetCode(ErrorCode)
					.SetPath(mappingContext.MiddlewareContext.Path)
					.SetMessage(mappingContext.ValidationFailure.ErrorMessage);
			}

			public static void Extensions(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
			{
				errorBuilder
					.SetExtension(Keys.ValidatorKey, mappingContext.ValidationFailure.ErrorCode)
					.SetExtension(Keys.InputFieldKey, mappingContext.InputField.Name)
					.SetExtension(Keys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
					.SetExtension(Keys.SeverityKey, mappingContext.ValidationFailure.Severity)
					.SetExtension(Keys.AttemptedValueKey, mappingContext.ValidationFailure.AttemptedValue);
			}
		}

		public static class ValidationFactories
		{
			public static IEnumerable<IValidator> Default(ValidatorFactoryContext validatorFactoryContext)
			{
				var validatorType = validatorFactoryContext.MakeGenericValidatorType();

				return (IEnumerable<IValidator>)validatorFactoryContext.ServiceProvider.GetServices(validatorType);
			}
		}
	}
}
