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
			public const string WithEmptyNameAndAddress = "mutation { test(input: { name: \"\", address: \"\" }) }";

			public const string WithEmptyNameAndSecondInput =
				"mutation { test(input: { name: \"\" }, input2: { name: \"\" }) }";
		}

		public static ValueTask<IRequestExecutor> CreateRequestExecutor(Action<IRequestExecutorBuilder> configure)
		{
			var services = new ServiceCollection();

			var executorBuilder = services.AddGraphQL().AddQueryType<TestQuery>();

			configure.Invoke(executorBuilder);

			return executorBuilder.BuildRequestExecutorAsync();
		}
	}
}
