using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class UseValidators
	{
		[Fact]
		public async Task UseValidators_Generic()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(new TestMutation(arg =>
					arg.UseFluentValidation(opt => opt.UseValidators<IValidator<TestPersonInput>>())))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_NonGeneric()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(new TestMutation(arg =>
					arg.UseFluentValidation(opt => opt.UseValidators(typeof(IValidator<TestPersonInput>)))))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameter()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(new TestMutation(arg =>
					arg.UseFluentValidation(opt => opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>())))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameterAndValidationStrategy()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(new TestMutation(arg =>
					arg.UseFluentValidation(opt =>
						opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
						{
							strategy.IncludeProperties(x => x.Name, x => x.Address);
						}))))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameterAndPartialValidationStrategy()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(new TestMutation(arg =>
					arg.UseFluentValidation(opt =>
						opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
						{
							strategy.IncludeProperties(x => x.Name);
						}))))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message));
		}
	}
}
