using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class UsingDefaults
	{
		[Fact]
		public async Task ExtensionCode()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			result.AssertDefaultErrorMapper(
				"NotEmptyValidator",
				NotEmptyNameValidator.Message);
		}

		[Fact]
		public async Task UseMultipleValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal("NotEmptyValidator", name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				},
				address =>
				{
					Assert.Equal("NotEmptyValidator", address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				});
		}

		[Fact]
		public async Task UseValidatorByConvention()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal("NotEmptyValidator", name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message1, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				},
				name =>
				{
					Assert.Equal("NotEmptyValidator", name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message2, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				});
		}
	}
}
