using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	public class FailedValidation : ValidationResult
	{
		public IDictionary<string, List<ValidationError>>? Errors { get; set; }
	}

	public class FailedValidationType : ObjectType<FailedValidation>
	{
		private readonly string name;
		private readonly string[] arguments;

		public FailedValidationType(string name, string[] arguments)
		{
			this.name = name;
			this.arguments = arguments;
		}

		protected override void Configure(IObjectTypeDescriptor<FailedValidation> descriptor)
		{
			descriptor.Name(name + "FailedValidation");

			descriptor.Field(x => x.Errors).Ignore();

			foreach (var argument in arguments)
			{
				descriptor.Field(argument)
					.Type<ListType<ValidationErrorType>>()
					.Resolve(context =>
					{
						var argumentToValidationErrors = context.Parent<FailedValidation>().Errors;

						return argumentToValidationErrors != null
							&& argumentToValidationErrors.TryGetValue(argument, out var errors)
								? errors
								: null;
					});
			}
		}
	}

	public class ValidationError
	{
		public string? Property { get; set; }
	}

	public class ValidationErrorType : ObjectType<ValidationError>
	{
		protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor)
		{
			descriptor.Field(x => x.Property);
		}
	}

	public class ValidationResult
	{
	}

	public class ValidationResultType<TSuccess> : UnionType<ValidationResult>
		where TSuccess : ObjectType
	{
		private readonly string name;
		private readonly string[] arguments;

		public ValidationResultType(string name, string[] arguments)
		{
			this.name = name;
			this.arguments = arguments;
		}

		protected override void Configure(IUnionTypeDescriptor descriptor)
		{
			descriptor.Name(name + "ValidationFailed");

			descriptor.Type<TSuccess>();
			descriptor.Type(new FailedValidationType(name, arguments));
		}
	}

	internal static class ValidationFieldMiddleware
	{
		public static FieldDelegate Use(FieldDelegate next)
		{
			return async middlewareContext =>
			{
				var argumentNodes = middlewareContext.Selection.SyntaxNode.Arguments;

				var validationErrorsMap = new Dictionary<string, List<ValidationError>>();

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

							var errors = new List<ValidationError>();

							for (var errorIndex = 0; errorIndex < validationResult.Errors.Count; errorIndex++)
							{
								var validationFailure = validationResult.Errors[errorIndex];

								var errorBuilder = ErrorBuilder.New();

								argumentOptions.ErrorMapper!.Invoke(
									errorBuilder,
									new ErrorMappingContext(middlewareContext, argument, validationResult, validationFailure));

								errors.Add(new ValidationError
								{
									Property = validationFailure.PropertyName
								});
							}

							validationErrorsMap.Add(argument.Name, errors);
						}
					}
				}

				if (validationErrorsMap is { Count: > 0 })
				{
					middlewareContext.Result = new FailedValidation
					{
						Errors = validationErrorsMap
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
