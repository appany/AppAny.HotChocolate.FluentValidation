using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class OverrideUseFluentValidation
	{
		[Fact]
		public async Task Should_UseOnlyDefaultErrorMapper()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithExtendedDetails())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseDefaultErrorMapper();
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseDefaultAndExtensionsErrorMapper()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseDefaultErrorMapperWithDetails();
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				},
				validator =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ValidatorKey, validator.Key);
					Assert.Equal(nameof(NotEmptyValidator), validator.Value);
				},
				argument =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ArgumentKey, argument.Key);
					Assert.Equal(new NameString("input"), argument.Value);
				},
				property =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.PropertyKey, property.Key);
					Assert.Equal("Name", property.Value);
				},
				severity =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.SeverityKey, severity.Key);
					Assert.Equal(Severity.Error, severity.Value);
				});
		}

		[Fact]
		public async Task Should_UseValidatorOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<NotEmptyNameValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseCustomValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field => field.Argument("input",
						arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
						{
							opt.UseInputValidators(async context =>
							{
								var argumentValue = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

								return await new NotEmptyNameValidator().ValidateAsync(argumentValue);
							});
						})))),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<NotEmptyNameValidator>()
									.UseValidator<NotEmptyAddressValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				address =>
				{
					Assert.Equal(ValidationDefaults.Code, address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators_ExplicitInputType()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<TestPersonInput, NotEmptyNameValidator>()
									.UseValidator<TestPersonInput, NotEmptyAddressValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				address =>
				{
					Assert.Equal(ValidationDefaults.Code, address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators_WithValidationStrategy_IgnoreAddress()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<NotEmptyNameValidator>()
									.UseValidator<TestPersonInput, NotEmptyAddressValidator>(strategy =>
										// Validates address, but includes only name
										strategy.IncludeProperties(input => input.Name));
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators_SameProperty()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<NotEmptyNameValidator>()
									.UseValidator<NotEmptyNameValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseSingleCustomValidator_DoubleProperty()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(fv =>
							{
								fv.UseValidator<DoubleNotEmptyNameValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message1, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message2, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_Execute_SkipValidation_WithValidatorOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation().UseValidator<NotEmptyNameValidator>();
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_SkipValidation_WithCustomValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation()
									.UseInputValidators(async context =>
									{
										var argumentValue = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

										return await new NotEmptyNameValidator().ValidateAsync(argumentValue);
									});
							}));
					})),
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Use_Multiple_InputValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field => field
						.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>()
							.UseFluentValidation(opt =>
							{
								opt.UseDefaultInputValidator(async context =>
								{
									var validator = new NotEmptyAddressValidator();

									var argumentValue = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

									return await validator
										.ValidateAsync(argumentValue, context.MiddlewareContext.RequestAborted)
										.ConfigureAwait(false);
								});
							})))),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				address =>
				{
					Assert.Equal(ValidationDefaults.Code, address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}
	}
}
