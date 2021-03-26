using System;
using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public static class TestSetup
  {
    public static class Mutations
    {
      public const string WithEmptyName = "mutation { test(input: { name: \"\" }) }";
      public const string WithNullInput = "mutation { test(input: null) }";
      public const string WithEmptyNameAndAddress = "mutation { test(input: { name: \"\", address: \"\" }) }";

      public const string WithMultipleInputsEmptyName =
        "mutation { test(input: { name: \"\" }, input2: { name: \"\" }) }";

      public static string WithName(string name)
      {
        return $"mutation {{ test(input: {{ name: \"{name}\" }}) }}";
      }

      public static string WithAddress(string address)
      {
        return $"mutation {{ test(input: {{ name: \"\", address: \"{address}\" }}) }}";
      }
    }

    public static ValueTask<IRequestExecutor> CreateRequestExecutor(
      Action<IRequestExecutorBuilder> configureExecutor,
      Action<IServiceCollection>? configureServices = null)
    {
      var services = new ServiceCollection();

      var executorBuilder = services.AddGraphQL().AddQueryType<TestQuery>();

      configureExecutor.Invoke(executorBuilder);

      configureServices?.Invoke(services);

      return executorBuilder.BuildRequestExecutorAsync();
    }
  }
}
