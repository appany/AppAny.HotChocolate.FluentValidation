global using System;
global using System.Threading.Tasks;
global using HotChocolate;
global using HotChocolate.Types;
global using HotChocolate.Resolvers;
global using FluentValidation;
global using FluentValidation.Results;

using System.Runtime.CompilerServices;
using FluentValidation.Internal;
using HotChocolate.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class ValidationDefaults
  {
    /// <summary>
    ///   Default <see cref="IHasContextData.ContextData" /> key for <see cref="ValidationOptions" />
    /// </summary>
    public const string ValidationOptionsKey = "ValidationOptions";

    /// <summary>
    ///   Default <see cref="IHasContextData.ContextData" /> key for <see cref="ArgumentValidationOptions" />
    /// </summary>
    public const string ArgumentOptionsKey = "ArgumentValidationOptions";

    /// <summary>
    ///   Default <see cref="IHasContextData.ContextData" /> key for <see cref="ObjectFieldValidationOptions" />
    /// </summary>
    public const string ObjectFieldOptionsKey = "ObjectFieldValidationOptions";

    /// <summary>
    ///   Default validation field middleware
    /// </summary>
    public static FieldMiddleware Middleware { get; } = ValidationMiddlewares.Field;

    /// <summary>
    ///   Default HotChocolate interceptors
    /// </summary>
    public static class Interceptors
    {
      public static OnCompleteType OnBeforeCompleteType { get; } = ValidationInterceptors.OnBeforeCompleteType;
      public static OnAfterSchemaCreate OnAfterSchemaCreate { get; } = ValidationInterceptors.OnAfterSchemaCreate;
    }

    /// <summary>
    ///   Default graphql error extensions keys
    /// </summary>
    public static class ExtensionKeys
    {
      public const string CodeKey = "code";
      public const string FieldKey = "field";
      public const string ArgumentKey = "argument";
      public const string PropertyKey = "property";
      public const string SeverityKey = "severity";
      public const string AttemptedValueKey = "attemptedValue";
      public const string CustomStateKey = "customState";
      public const string FormattedMessagePlaceholderValuesKey = "formattedMessagePlaceholderValues";
    }

    /// <summary>
    ///   Default <see cref="FluentValidation.SkipValidation" /> implementations
    /// </summary>
    public static class SkipValidation
    {
      /// <summary>
      ///   Default <see cref="SkipValidation" /> implementation. Never skips validation
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ValueTask<bool> Default(SkipValidationContext skipValidationContext)
      {
        return new(false);
      }

      /// <summary>
      ///   Always skip <see cref="SkipValidation" /> implementation
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ValueTask<bool> Skip(SkipValidationContext skipValidationContext)
      {
        return new(true);
      }
    }

    /// <summary>
    ///   Default <see cref="MapError" /> implementations
    /// </summary>
    public static class ErrorMappers
    {
      /// <summary>
      ///   Maps graphql error code, path and message
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Default(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
      {
        errorBuilder
          .SetCode(mappingContext.ValidationFailure.ErrorCode)
          .SetPath(mappingContext.MiddlewareContext.Path)
          .SetMessage(mappingContext.ValidationFailure.ErrorMessage);
      }

      /// <summary>
      ///   Maps useful extensions about input field, property, used validator, invalid value and severity
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Details(IErrorBuilder errorBuilder, ErrorMappingContext mappingContext)
      {
        errorBuilder
          .SetExtension(ExtensionKeys.FieldKey, mappingContext.MiddlewareContext.Selection.Field.Name)
          .SetExtension(ExtensionKeys.ArgumentKey, mappingContext.Argument.Name)
          .SetExtension(ExtensionKeys.PropertyKey, mappingContext.ValidationFailure.PropertyName)
          .SetExtension(ExtensionKeys.SeverityKey, mappingContext.ValidationFailure.Severity);
      }

      /// <summary>
      ///   Maps custom state and formatted message placeholder values
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
    ///   Default <see cref="ValidateInput" /> implementations
    /// </summary>
    public static class InputValidators
    {
      /// <summary>
      ///   Default <see cref="ValidateInput" /> implementation
      /// </summary>
      public static Task<ValidationResult?> Default(InputValidatorContext inputValidatorContext)
      {
        var argumentValue = Steps.ArgumentValue<object>(inputValidatorContext);

        if (argumentValue is null)
        {
          return Task.FromResult<ValidationResult?>(null);
        }

        var validationContext = Steps.ValidationContext(inputValidatorContext, argumentValue);

        var validatorType = inputValidatorContext.Argument.GetGenericValidatorType();

        return Steps.Validators(inputValidatorContext, validationContext, validatorType);
      }

      /// <summary>
      ///   Default <see cref="ValidateInput" /> steps implementations
      /// </summary>
      public static class Steps
      {
        /// <summary>
        ///   Default <see cref="GetArgumentValue{TInput}" /> implementation
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TInput? ArgumentValue<TInput>(InputValidatorContext inputValidatorContext)
        {
          return inputValidatorContext
            .MiddlewareContext
            .ArgumentValue<TInput?>(inputValidatorContext.Argument.Name);
        }

        /// <summary>
        ///   Default <see cref="GetValidationContext{TInput}" /> implementation
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IValidationContext ValidationContext<TInput>(
          InputValidatorContext inputValidatorContext,
          TInput argumentValue)
        {
          return new ValidationContext<TInput>(argumentValue);
        }

        /// <summary>
        ///   Default <see cref="GetValidationContext{TInput}" /> implementation with <see cref="ValidationStrategy{T}" />
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GetValidationContext<TInput> ValidationContextWithStrategy<TInput>(
          Action<InputValidatorContext, ValidationStrategy<TInput>> validationStrategy)
        {
          return (inputValidatorContext, argumentValue) =>
          {
            // TODO: ValidationContext aliasing hack
            return global::FluentValidation.ValidationContext<TInput>.CreateWithOptions(
              argumentValue,
              strategy => validationStrategy(inputValidatorContext, strategy));
          };
        }

        /// <summary>
        ///   Default <see cref="GetValidationResult" /> implementation
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<ValidationResult?> Validator(
          InputValidatorContext inputValidatorContext,
          IValidationContext validationContext,
          Type validatorType)
        {
          var validator = (IValidator)inputValidatorContext.MiddlewareContext
            .Services
            .GetRequiredService(validatorType);

          return validator.ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted);
        }

        /// <summary>
        ///   Default <see cref="GetValidationResult" /> implementation for multiple <see cref="IValidator" />
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<ValidationResult?> Validators(
          InputValidatorContext inputValidatorContext,
          IValidationContext validationContext,
          Type validatorType)
        {
          var validators = (IValidator[])inputValidatorContext.MiddlewareContext.Services.GetServices(validatorType);

          ValidationResult? validationResult = null;

          for (var validatorIndex = 0; validatorIndex < validators.Length; validatorIndex++)
          {
            var validator = validators[validatorIndex];

            var validatorResult = await validator
              .ValidateAsync(validationContext, inputValidatorContext.MiddlewareContext.RequestAborted)
              .ConfigureAwait(false);

            // ValidationResult failures are bound to each ValidationContext
            validationResult = validatorResult;
          }

          return validationResult;
        }
      }
    }

    /// <summary>
    ///   Default <see cref="ValidationStrategy{T}" /> implementations
    /// </summary>
    public static class ValidationStrategies
    {
      /// <summary>
      ///   Doing nothing by default.
      ///   To override validation strategy use
      ///   <see cref="UseValidatorExtensions.UseValidator{TValidator}(ArgumentValidationBuilder)" />
      /// </summary>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Default<TInput>(ValidationStrategy<TInput> validationStrategy)
      {
      }
    }
  }
}
