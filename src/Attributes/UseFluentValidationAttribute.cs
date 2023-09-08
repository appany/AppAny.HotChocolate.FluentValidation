using System.Reflection;
using HotChocolate.Types.Descriptors;

namespace AppAny.HotChocolate.FluentValidation
{
  /// <summary>
  ///   Use default validation options.
  ///   To override options use Code-first approach <see cref="ArgumentDescriptorExtensions" />
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class UseFluentValidationAttribute : ArgumentDescriptorAttribute
  {

    protected override void OnConfigure(
      IDescriptorContext descriptorContext,
      IArgumentDescriptor argumentDescriptor,
      ParameterInfo parameterInfo)
    {
      var fluentValidationAttributes = parameterInfo.GetCustomAttributes<FluentValidationAttribute>();

      argumentDescriptor.UseFluentValidation(options =>
      {
        foreach (var fluentValidationAttribute in fluentValidationAttributes)
        {
          fluentValidationAttribute.Configure(options);
        }
      });
    }
  }
}
