using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class UsingAttributes
  {
    [Fact]
    public async Task AttributeConfiguration()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseFluentValidationMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task DefaultInputValidator()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseDefaultInputValidatorMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task DefaultErrorMapper()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseDefaultErrorMapperMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task DefaultErrorMapperWithDetails()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseDefaultErrorMapperWithDetailsMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
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
    public async Task DefaultErrorMapperWithExtendedDetails()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseDefaultErrorMapperWithExtendedDetailsMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
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
    public async Task AttributeConfigurationWithCustomValidator()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task AttributeConfigurationWithSkipValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestSkipValidationMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertSuceessResult();
    }

    [Fact]
    public async Task AttributeConfigurationWithCustomSkipValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestCustomSkipValidationMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task CustomSkipValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestCustomSkipValidationMutation());
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("Custom")));

      result.AssertSuceessResult();
    }

    [Fact]
    public async Task Should_Use_AttributeConfiguration_CustomValidators()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorsMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        },
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorWithValidationStrategy()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorWithValidationStrategyMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorWithIncludeAllRuleSets()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorWithIncludeAllRuleSetsMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorWithIncludeRulesNotInRuleSet()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorWithIncludeRulesNotInRuleSetMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorWithIncludeRuleSets()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorWithIncludeRuleSetsMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertSuceessResult();
    }

    [Fact]
    public async Task UseValidatorWithValidationStrategyAddress()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorWithValidationStrategyAddressMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertSuceessResult();
    }

    [Fact]
    public async Task UseValidatorsWithValidationStrategy()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorsWithValidationStrategyMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(NotEmptyNameValidator.Message, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorsWithValidationStrategyAddress()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestUseValidatorsWithValidationStrategyAddressMutation());
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertSuceessResult();
    }
  }
}
