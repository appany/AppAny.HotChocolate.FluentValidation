namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public static class TestMutations
	{
		public const string EmptyName = "mutation { test(input: { name: \"\" }) }";
		public const string EmptyNameAndAddress = "mutation { test(input: { name: \"\", address: \"\" }) }";
	}
}
