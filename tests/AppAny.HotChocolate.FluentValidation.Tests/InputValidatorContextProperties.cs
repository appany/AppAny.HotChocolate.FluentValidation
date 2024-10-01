using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class InputValidatorContextProperties
  {
    [Fact]
    public async Task AddFluentValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt =>
            {
              opt.UseDefaultErrorMapper()
                .UseInputValidators(context =>
                {
                  Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

                  return ValidationDefaults.InputValidators.Default(context);
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
        NotEmptyNameValidator.Message);
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
                arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
                {
                  opt.UseInputValidators(context =>
                  {
                    Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

                    return ValidationDefaults.InputValidators.Default(context);
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
        NotEmptyNameValidator.Message);
    }
  }
}
