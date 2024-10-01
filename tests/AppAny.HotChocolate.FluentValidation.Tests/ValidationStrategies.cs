using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class ValidationStrategies
  {
    [Fact]
    public async Task DynamicValidationStrategy()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidator<TestPersonInput, IValidator<TestPersonInput>>((context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties(x => x.Name);
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties(x => x.Address);
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }

    [Fact]
    public async Task DynamicValidationStrategy_UseValidator_Strategy()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidator<IValidator<TestPersonInput>>((context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties("Name");
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties("Address");
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }

    [Fact]
    public async Task DynamicValidationStrategy_UseValidatorsStrategy()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidators<IValidator<TestPersonInput>>((context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties("Name");
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties("Address");
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }

    [Fact]
    public async Task DynamicValidationStrategy_UseValidators()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>((context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties(x => x.Name);
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties(x => x.Address);
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }

    [Fact]
    public async Task DynamicValidationStrategy_Typed()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidator(typeof(IValidator<TestPersonInput>), (context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties("Name");
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties("Address");
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }

    [Fact]
    public async Task DynamicValidationStrategy_UseValidators_Typed()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseValidators(typeof(IValidator<TestPersonInput>), (context, strategy) =>
                {
                  var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

                  if (input.Name == "WithName")
                  {
                    strategy.IncludeProperties("Name");
                  }

                  if (input.Address == "WithAddress")
                  {
                    strategy.IncludeProperties("Address");
                  }
                });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result1 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result1.AssertNullResult();

      result1.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);

      var result2 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

      result2.AssertSuceessResult();

      var result3 = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

      result3.AssertSuceessResult();
    }
  }
}
