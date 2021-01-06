using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public static class ServiceCollectionExtensions
	{
		public static IRequestExecutorBuilder AddTestGraphQL(this IServiceCollection services)
		{
			return services.AddGraphQL().AddQueryType<TestQuery>();
		}
	}
}
