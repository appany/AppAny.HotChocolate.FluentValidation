using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace AppAny.HotChocolate.FluentValidation
{
  internal sealed class ValidationInterceptors
  {
    public static void OnBeforeCompleteType(
      ITypeCompletionContext completionContext,
      DefinitionBase? definition,
      IDictionary<string, object?> contextData)
    {
      if (definition is not ObjectTypeDefinition objectTypeDefinition)
      {
        return;
      }

      var validationOptions = completionContext.ContextData.GetValidationOptions();

      foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
      {
        var argumentOptions = objectFieldDefinition.Arguments
          .Where(argument => argument.ContextData.ShouldValidateArgument())
          .Select(argument => argument.ContextData.GetArgumentOptions())
          .ToList();

        if (argumentOptions is { Count: > 0 })
        {
          foreach (var options in argumentOptions)
          {
            options.MergeValidationOptions(validationOptions);
          }

          objectFieldDefinition.MiddlewareComponents.Insert(0, ValidationDefaults.Middleware);
          objectFieldDefinition.ContextData.CreateObjectFieldOptions();
        }
      }
    }

    public static void OnAfterSchemaCreate(IDescriptorContext context, ISchema schema)
    {
      foreach (var objectField in schema.Types.OfType<IObjectType>().SelectMany(type => type.Fields))
      {
        foreach (var argument in objectField.Arguments.Where(arg => arg.ContextData.ShouldValidateArgument()))
        {
          var objectOptions = objectField.ContextData.TryGetObjectFieldOptions();

          objectOptions?.Arguments.Add(argument.Name, argument);
        }
      }
    }
  }
}
