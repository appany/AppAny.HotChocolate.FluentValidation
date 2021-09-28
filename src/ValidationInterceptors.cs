using System.Collections.Generic;
using System.Linq;
using HotChocolate.Configuration;
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

          objectFieldDefinition.ContextData.CreateObjectFieldOptions();

          objectFieldDefinition.MiddlewareDefinitions.Insert(
            index: 0,
            new FieldMiddlewareDefinition(ValidationDefaults.Middleware));
        }
      }
    }

    public static void OnAfterSchemaCreate(IDescriptorContext descriptorContext, ISchema schema)
    {
      foreach (var objectField in schema.Types.OfType<IObjectType>().SelectMany(type => type.Fields))
      {
        var objectOptions = objectField.ContextData.TryGetObjectFieldOptions();

        foreach (var argument in objectField.Arguments.Where(arg => arg.ContextData.ShouldValidateArgument()))
        {
          objectOptions?.Arguments.Add(argument.Name, argument);
        }
      }
    }
  }
}
