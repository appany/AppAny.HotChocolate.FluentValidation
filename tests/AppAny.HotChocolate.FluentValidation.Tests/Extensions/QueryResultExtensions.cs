using HotChocolate.Execution;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public static class QueryResultExtensions
	{
		public static void AssertNullResult(this QueryResult result)
		{
			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Null(value);
		}
	}
}
