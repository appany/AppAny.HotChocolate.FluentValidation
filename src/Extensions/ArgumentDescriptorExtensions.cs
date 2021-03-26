using System;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
  public static class ArgumentDescriptorExtensions
  {
    /// <summary>
    /// Configures argument for validation
    /// </summary>
    public static IArgumentDescriptor UseFluentValidation(
      this IArgumentDescriptor argumentDescriptor,
      Action<ArgumentValidationBuilder>? configure = null)
    {
      argumentDescriptor.Extend().OnBeforeCreate(argumentDefinition =>
      {
        var options = argumentDefinition.ContextData.GetOrCreateArgumentOptions();

        configure?.Invoke(new DefaultArgumentValidationBuilder(options));
      });

      return argumentDescriptor;
    }
  }
}
