using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class OverrideErrorMappers
  {
    [Fact]
    public async Task AddFluentValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt =>
            {
              opt.UseDefaultErrorMapper(
                (errorBuilder, _) =>
                {
                  errorBuilder.SetExtension("test", "test");
                });
            })
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message,
        test =>
        {
          Assert.Equal("test", test.Key);
          Assert.Equal("test", test.Value);
        });
    }

    [Fact]
    public async Task UseFluentValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(
                  opt =>
                  {
                    opt.UseDefaultErrorMapper(
                      (errorBuilder, _) =>
                      {
                        errorBuilder.SetExtension("test", "test");
                      });
                  }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message,
        test =>
        {
          Assert.Equal("test", test.Key);
          Assert.Equal("test", test.Value);
        });
    }

    [Fact]
    public async Task UseDefaultErrorMapperWithDetails()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithDetails())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg =>
              {
                arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation();
              });
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

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
        });
    }

    [Fact]
    public async Task UseDefaultErrorMapperWithDetailsWithOperationName()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithDetails())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg =>
              {
                arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation();
              });
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithOperationNameEmptyName));

      result.AssertNullResult();

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
              Assert.Equal("OperationName", operation.Value);
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

    [Fact]
    public async Task UseDefaultErrorMapperWithExtendedDetails()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithExtendedDetails())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg =>
              {
                arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation();
              });
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

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
            },
            attemptedValue =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.AttemptedValueKey, attemptedValue.Key);
              Assert.Equal("", attemptedValue.Value);
            },
            customState =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CustomStateKey, customState.Key);
              Assert.Null(customState.Value);
            },
            formattedMessagePlaceholerValues =>
            {
              Assert.Equal(
                ValidationDefaults.ExtensionKeys.FormattedMessagePlaceholderValuesKey,
                formattedMessagePlaceholerValues.Key);

              var values = Assert.IsType<Dictionary<string, object>>(formattedMessagePlaceholerValues.Value);

              Assert.Collection(values,
                propertyName =>
                {
                  Assert.Equal("PropertyName", Assert.IsType<string>(propertyName.Key));
                  Assert.Equal("Name", Assert.IsType<string>(propertyName.Value));
                },
                propertyValue =>
                {
                  Assert.Equal("PropertyValue", Assert.IsType<string>(propertyValue.Key));
                  Assert.Equal("", Assert.IsType<string>(propertyValue.Value));
                },
                propertyValue =>
                {
                  Assert.Equal("PropertyPath", Assert.IsType<string>(propertyValue.Key));
                  Assert.Equal("Name", Assert.IsType<string>(propertyValue.Value));
                });
            });
        });
    }

    [Fact]
    public async Task UseDefaultErrorMapperWithCustomExtendedDetails()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithExtendedDetails((errorBuilder, _) =>
            {
              errorBuilder.SetExtension("test", "extension");
            }))
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg =>
              {
                arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation();
              });
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

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
            },
            attemptedValue =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.AttemptedValueKey, attemptedValue.Key);
              Assert.Equal("", attemptedValue.Value);
            },
            customState =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CustomStateKey, customState.Key);
              Assert.Null(customState.Value);
            },
            formattedMessagePlaceholerValues =>
            {
              Assert.Equal(
                ValidationDefaults.ExtensionKeys.FormattedMessagePlaceholderValuesKey,
                formattedMessagePlaceholerValues.Key);

              var values = Assert.IsType<Dictionary<string, object>>(formattedMessagePlaceholerValues.Value);

              Assert.Collection(values,
                propertyName =>
                {
                  Assert.Equal("PropertyName", Assert.IsType<string>(propertyName.Key));
                  Assert.Equal("Name", Assert.IsType<string>(propertyName.Value));
                },
                propertyValue =>
                {
                  Assert.Equal("PropertyValue", Assert.IsType<string>(propertyValue.Key));
                  Assert.Equal("", Assert.IsType<string>(propertyValue.Value));
                },
                propertyValue =>
                {
                  Assert.Equal("PropertyPath", Assert.IsType<string>(propertyValue.Key));
                  Assert.Equal("Name", Assert.IsType<string>(propertyValue.Value));
                });
            },
            test =>
            {
              Assert.Equal("test", test.Key);
              Assert.Equal("extension", test.Value);
            });
        });
    }

    [Fact]
    public async Task MultipleErrorMappers()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseDefaultErrorMapper()
                  .UseDefaultErrorMapperWithDetails();
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

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
        });
    }
  }
}
