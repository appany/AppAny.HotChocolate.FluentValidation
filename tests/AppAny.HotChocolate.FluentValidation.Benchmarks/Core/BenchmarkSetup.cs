using System;
using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	public static class BenchmarkSetup
	{
		public static class Mutations
		{
			public const string WithEmptyName = "mutation { test(input: { name: \"\" }) }";
			public const string WithEmptyInput = "mutation { test() }";
			public const string WithNullInput = "mutation { test(input: null) }";
		}

		public static ValueTask<IRequestExecutor> CreateRequestExecutor(Action<IRequestExecutorBuilder> configure)
		{
			var services = new ServiceCollection();

			var executorBuilder = services.AddGraphQL().AddQueryType<TestQueryType>();

			configure.Invoke(executorBuilder);

			return executorBuilder.BuildRequestExecutorAsync();
		}
	}
}
