namespace AppAny.HotChocolate.FluentValidation
{
  internal static class ValidationMiddlewares
  {
    public static FieldDelegate Field(FieldDelegate next)
    {
      return async middlewareContext =>
      {
        var argumentNodes = middlewareContext.Selection.SyntaxNode.Arguments;

        if (argumentNodes is { Count: > 0 })
        {
          var objectFieldOptions = middlewareContext.Selection.Field.ContextData.GetObjectFieldOptions();

          for (var nodeIndex = 0; nodeIndex < argumentNodes.Count; nodeIndex++)
          {
            var argumentNode = argumentNodes[nodeIndex];

            var argument = objectFieldOptions.Arguments.TryGetArgument(argumentNode.Name.Value);

            if (argument is null)
            {
              continue;
            }

            var argumentOptions = argument.ContextData.GetArgumentOptions();

            // TODO: Nullable hack. Can't be null at runtime
            var shouldSkipValidation = await argumentOptions.SkipValidation!
              .Invoke(new SkipValidationContext(middlewareContext, argument))
              .ConfigureAwait(false);

            if (shouldSkipValidation)
            {
              continue;
            }

            // TODO: Nullable hack. Can't be null at runtime
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

              // TODO: Nullable hack. Can't be null at runtime
              var errorMapper = argumentOptions.ErrorMapper!;

              for (var errorIndex = 0; errorIndex < validationResult.Errors.Count; errorIndex++)
              {
                var validationFailure = validationResult.Errors[errorIndex];

                var errorBuilder = ErrorBuilder.New();

                errorMapper.Invoke(
                  errorBuilder,
                  new ErrorMappingContext(middlewareContext, argument, validationResult, validationFailure));

                middlewareContext.ReportError(errorBuilder.Build());
              }
            }
          }
        }

        if (middlewareContext.HasErrors is false)
        {
          await next(middlewareContext).ConfigureAwait(false);
        }
      };
    }
  }
}
