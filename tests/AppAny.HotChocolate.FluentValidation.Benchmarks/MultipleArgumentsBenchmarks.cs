using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
  [MemoryDiagnoser]
  public class MultipleArgumentsBenchmarks
  {
    private IRequestExecutor withFiveArguments = default!;
    private IRequestExecutor withoutFiveArguments = default!;
    private IRequestExecutor withoutSingleArgument = default!;
    private IRequestExecutor withSingleArgument = default!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
      withoutSingleArgument = await BenchmarkSetup.CreateRequestExecutor(
        builder => builder.AddMutationType(new TestMutationType(field => field
          .Argument("input", arg => arg.Type<TestInputType>()))));

      withSingleArgument = await BenchmarkSetup.CreateRequestExecutor(
        builder => builder.AddFluentValidation()
          .AddMutationType(new TestMutationType(field => field
            .Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())))
          .Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());

      withoutFiveArguments = await BenchmarkSetup.CreateRequestExecutor(
        builder => builder.AddMutationType(new TestMutationType(field => field
          .Argument("input", arg => arg.Type<TestInputType>())
          .Argument("input2", arg => arg.Type<TestInputType>())
          .Argument("input3", arg => arg.Type<TestInputType>())
          .Argument("input4", arg => arg.Type<TestInputType>())
          .Argument("input5", arg => arg.Type<TestInputType>()))));

      withFiveArguments = await BenchmarkSetup.CreateRequestExecutor(
        builder => builder.AddFluentValidation()
          .AddMutationType(new TestMutationType(field => field
            .Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())
            .Argument("input2", arg => arg.Type<TestInputType>().UseFluentValidation())
            .Argument("input3", arg => arg.Type<TestInputType>().UseFluentValidation())
            .Argument("input4", arg => arg.Type<TestInputType>().UseFluentValidation())
            .Argument("input5", arg => arg.Type<TestInputType>().UseFluentValidation())))
          .Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());
    }

    [Benchmark]
    public Task RunWithoutSingleArgument()
    {
      return withoutSingleArgument.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
    }

    [Benchmark]
    public Task RunWithSingleArgument()
    {
      return withSingleArgument.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
    }

    [Benchmark]
    public Task RunWithoutFiveArguments()
    {
      return withoutFiveArguments.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
    }

    [Benchmark]
    public Task RunWithFiveArguments()
    {
      return withFiveArguments.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
    }
  }
}
