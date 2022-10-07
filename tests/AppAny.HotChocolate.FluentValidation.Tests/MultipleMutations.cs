using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class MultipleMutations
  {
    [Fact]
    public async Task Multiple()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(options => options.UseDefaultErrorMapperWithDetails())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.MultipleMutationsWithEmptyName));

      Assert.Collection(result.Data!,
        data =>
        {
          Assert.Equal("a", data.Key);
          Assert.Null(data.Value);
        },
        data =>
        {
          Assert.Equal("b", data.Key);
          Assert.Null(data.Value);
        });

      Assert.Collection(result.Errors!,
        error =>
        {
          Assert.Equal("NotEmptyValidator", error.Code);
          Assert.Equal(NotEmptyNameValidator.Message, error.Message);

          Assert.Collection(error.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            },
            operation =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.OperationKey, operation.Key);
              Assert.Null(operation.Value);
            },
            field =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.FieldKey, field.Key);
              Assert.Equal("test", field.Value);
            },
            argument =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.ArgumentKey, argument.Key);
              Assert.Equal("input", argument.Value);
            },
            property =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.PropertyKey, property.Key);
              Assert.Equal("Name", property.Value);
            },
            severity =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.SeverityKey, severity.Key);
              Assert.Equal(Severity.Error, severity.Value);
            });
        },
        error =>
        {
          Assert.Equal("NotEmptyValidator", error.Code);
          Assert.Equal(NotEmptyNameValidator.Message, error.Message);

          Assert.Collection(error.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            },
            operation =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.OperationKey, operation.Key);
              Assert.Null(operation.Value);
            },
            field =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.FieldKey, field.Key);
              Assert.Equal("test", field.Value);
            },
            argument =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.ArgumentKey, argument.Key);
              Assert.Equal("input", argument.Value);
            },
            property =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.PropertyKey, property.Key);
              Assert.Equal("Name", property.Value);
            },
            severity =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.SeverityKey, severity.Key);
              Assert.Equal(Severity.Error, severity.Value);
            });
        });
    }
  }
}
