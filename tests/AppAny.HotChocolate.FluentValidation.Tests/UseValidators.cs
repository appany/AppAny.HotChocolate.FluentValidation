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
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg =>
						arg.UseFluentValidation(opt => opt.UseValidators<IValidator<TestPersonInput>>())))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_NonGeneric()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg =>
						arg.UseFluentValidation(opt => opt.UseValidators(typeof(IValidator<TestPersonInput>)))))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameter()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg =>
						arg.UseFluentValidation(opt => opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>())))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameterAndValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg =>
						arg.UseFluentValidation(opt =>
							opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
							{
								strategy.IncludeProperties(input => input.Name, input => input.Address);
							}))))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal("Address is empty", addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidators_GenericWithInputParameterAndPartialValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg =>
						arg.UseFluentValidation(opt =>
							opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
							{
								strategy.IncludeProperties(input => input.Name);
							}))))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal("Name is empty", nameIsEmpty.Message));
		}
	}
}
