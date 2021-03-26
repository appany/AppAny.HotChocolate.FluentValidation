using System.Threading.Tasks;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class CustomSkipValidationAttribute : SkipValidationAttribute
  {
    public const string SkipName = "Custom";

    protected override ValueTask<bool> SkipValidation(SkipValidationContext skipValidationContext)
    {
      var argumentValue = skipValidationContext
        .MiddlewareContext
        .ArgumentValue<TestPersonInput>(skipValidationContext.Argument.Name);

      return new ValueTask<bool>(argumentValue.Name == SkipName);
    }
  }

  public class TestCustomSkipValidationMutation : ObjectType
  {
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
      descriptor.Field<TestCustomSkipValidationMutation>(
        field => field.Test(default!)).Type<StringType>();
    }

    public string Test(
      [UseFluentValidation, CustomSkipValidation]
      TestPersonInput input)
    {
      return "test";
    }
  }
}
