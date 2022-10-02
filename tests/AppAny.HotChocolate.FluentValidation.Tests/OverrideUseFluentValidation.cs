using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class OverrideUseFluentValidation
  {
    [Fact]
    public async Task UseDefaultErrorMapperFieldOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithExtendedDetails())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseDefaultErrorMapper();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }

    [Fact]
    public async Task UseDefaultErrorMapperWithDetailsFieldOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseDefaultErrorMapperWithDetails();
                }));
            }));
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
    public async Task UseValidatorOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator<NotEmptyNameValidator>();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }

    [Fact]
    public async Task UseValidatorTypedOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator(typeof(NotEmptyNameValidator));
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }

    [Fact]
    public async Task UseInputValidatorOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field => field.Argument("input",
              arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseInputValidators(async context =>
                {
                  var argumentValue = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  return await new NotEmptyNameValidator().ValidateAsync(argumentValue);
                });
              }))));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }

    [Fact]
    public async Task MultipleUseValidatorOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator<NotEmptyNameValidator>()
                    .UseValidator<NotEmptyAddressValidator>();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

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
        address =>
        {
          Assert.Equal("NotEmptyValidator", address.Code);
          Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

          Assert.Collection(address.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task UseValidatorTypeOverride()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator<TestPersonInput, NotEmptyNameValidator>()
                    .UseValidator<TestPersonInput, NotEmptyAddressValidator>();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

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
        address =>
        {
          Assert.Equal("NotEmptyValidator", address.Code);
          Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

          Assert.Collection(address.Extensions!,
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
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator<NotEmptyNameValidator>()
                    .UseValidator<TestPersonInput, NotEmptyAddressValidator>(strategy =>
                      // Validates address, but includes only name
                      strategy.IncludeProperties(input => input.Name));
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

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
    public async Task UseValidatorTwice()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseValidator<NotEmptyNameValidator>()
                    .UseValidator<NotEmptyNameValidator>();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
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
    public async Task UseCustomValidator()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(fv =>
                {
                  fv.UseValidator<DoubleNotEmptyNameValidator>();
                }));
            }));
        },
        services =>
        {
          services.AddTransient<DoubleNotEmptyNameValidator>();
        });

      var result = Assert.IsType<QueryResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      Assert.Collection(result.Errors!,
        name =>
        {
          Assert.Equal("NotEmptyValidator", name.Code);
          Assert.Equal(DoubleNotEmptyNameValidator.Message1, name.Message);

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
          Assert.Equal(DoubleNotEmptyNameValidator.Message2, name.Message);

          Assert.Collection(name.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }

    [Fact]
    public async Task SkipValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.SkipValidation().UseValidator<NotEmptyNameValidator>();
                }));
            }));
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
    public async Task SkipValidationWithInputValidator()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input",
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.SkipValidation()
                    .UseInputValidators(async context =>
                    {
                      var argumentValue =
                        context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                      return await new NotEmptyNameValidator().ValidateAsync(argumentValue);
                    });
                }));
            }));
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
    public async Task UseDefaultInputValidatorWithCustom()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field => field
              .Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>()
                .UseFluentValidation(opt =>
                {
                  opt.UseDefaultInputValidator(async context =>
                  {
                    var validator = new NotEmptyAddressValidator();

                    var argumentValue = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                    return await validator
                      .ValidateAsync(argumentValue, context.MiddlewareContext.RequestAborted)
                      .ConfigureAwait(false);
                  });
                }))));
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
        },
        address =>
        {
          Assert.Equal("NotEmptyValidator", address.Code);
          Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

          Assert.Collection(address.Extensions!,
            code =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
              Assert.Equal("NotEmptyValidator", code.Value);
            });
        });
    }
  }
}
