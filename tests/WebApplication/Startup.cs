using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
	public class ValidationResultType<TOutputType> : UnionType
		where TOutputType : ObjectType
	{
		private readonly string field;
		private readonly Dictionary<string, string[]> arguments;

		public ValidationResultType(string field, Dictionary<string, string[]> arguments)
		{
			this.field = field;
			this.arguments = arguments;
		}

		protected override void Configure(IUnionTypeDescriptor descriptor)
		{
			descriptor.Name(field + "Result");

			descriptor.Type<TOutputType>();
			descriptor.Type(new ValidationErrorType(field, arguments));
		}
	}

	public class ValidationError
	{
	}

	public class ValidationErrorType : ObjectType<ValidationError>
	{
		private readonly string field;
		private readonly Dictionary<string, string[]> arguments;

		public ValidationErrorType(string field, Dictionary<string, string[]> arguments)
		{
			this.field = field;
			this.arguments = arguments;
		}

		protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor)
		{
			descriptor.Name(field + "Validation");

			foreach (var (argument, properties) in arguments)
			{
				descriptor.Field(argument)
					.Type(new ValidationArgumentType(field, argument, properties))
					.Resolve(new object());
			}
		}
	}

	public class ValidationArgumentType : ObjectType
	{
		private readonly string field;
		private readonly string argument;
		private readonly string[] properties;

		public ValidationArgumentType(string field, string argument, string[] properties)
		{
			this.field = field;
			this.argument = argument;
			this.properties = properties;
		}

		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Name(field + char.ToUpper(argument[0]) + argument[1..] + "Argument");

			foreach (var property in properties)
			{
				descriptor.Field(property)
					.Type<ListType<ValidationResultType>>()
					.Resolve(context => context.GetScopedValue<ValidationResult[]>($"{argument}:{property}"));
			}
		}
	}

	public class ValidationResult
	{
		public string Message { get; set; }
		public string Validator { get; set; }
		public string Severity { get; set; }
	}

	public class ValidationResultType : ObjectType<ValidationResult>
	{
		protected override void Configure(IObjectTypeDescriptor<ValidationResult> descriptor)
		{
			descriptor.Field(x => x.Message);
			descriptor.Field(x => x.Validator);
			descriptor.Field(x => x.Severity);
		}
	}

	public class CreateUserPayload
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
	}

	public class CreateUserPayloadType : ObjectType<CreateUserPayload>
	{
		protected override void Configure(IObjectTypeDescriptor<CreateUserPayload> descriptor)
		{
			descriptor.Field(x => x.Id);
			descriptor.Field(x => x.UserName);
			descriptor.Field(x => x.Email);
		}
	}

	public class CreateUserInput
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class CreateUserInputType : InputObjectType<CreateUserInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<CreateUserInput> descriptor)
		{
			descriptor.Field(x => x.UserName);
			descriptor.Field(x => x.Email);
			descriptor.Field(x => x.Password);
		}
	}

	public class UserResolvers
	{
		public CreateUserPayload CreateUser(CreateUserInput input)
		{
			return new()
			{
				Id = Guid.NewGuid().ToString("N"),
				Email = input.Email,
				UserName = input.UserName
			};
		}
	}

	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGraphQLServer()
				.AddQueryType(x => x.Name("Query").Field("test").Resolve("Works!"))
				.AddMutationType(x => x.Name("Mutation")
					.Field<UserResolvers>(r => r.CreateUser(default!))
					.Type(new ValidationResultType<CreateUserPayloadType>("CreateUser", new Dictionary<string, string[]>
					{
						["input"] = new[] { "userName", "email", "password" }
					}))
					.Argument("input", arg => arg.Type<CreateUserInputType>())
					.Use(next => async context =>
					{
						context.SetScopedValue("input:userName", new ValidationResult[]
						{
							new()
							{
								Message = "UserName is empty",
								Severity = "Warning",
								Validator = "ManualValidator"
							}
						});
						context.SetScopedValue("input:email", new ValidationResult[]
						{
							new()
							{
								Message = "Email is empty",
								Severity = "Warning",
								Validator = "ManualValidator"
							}
						});
						context.SetScopedValue("input:password", new ValidationResult[]
						{
							new()
							{
								Message = "Password is empty",
								Severity = "Warning",
								Validator = "ManualValidator"
							}
						});

						context.Result = new ValidationError();

						// await next(context);
					}));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGraphQL();
			});
		}
	}
}
