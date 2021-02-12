using System;
using System.Collections.Generic;
using AppAny.HotChocolate.FluentValidation;
using AppAny.HotChocolate.FluentValidation.Types;
using FluentValidation;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
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

	public class CreateProductPayload
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public class CreateProductPayloadType : ObjectType<CreateProductPayload>
	{
		protected override void Configure(IObjectTypeDescriptor<CreateProductPayload> descriptor)
		{
			descriptor.Field(x => x.Id);
			descriptor.Field(x => x.Name);
		}
	}

	public class CreateUserInput
	{
		public string UserName { get; set; }
		public string Email { get; set; }
	}

	public class CreateUserInputType : InputObjectType<CreateUserInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<CreateUserInput> descriptor)
		{
			descriptor.Field(x => x.UserName);
			descriptor.Field(x => x.Email);
		}
	}

	public class CreateUserInputValidator : AbstractValidator<CreateUserInput>
	{
		public CreateUserInputValidator()
		{
			RuleFor(x => x.UserName)
				.NotEqual("Invalid")
				.WithMessage("Invalid user name")
				.WithName("userName");

			RuleFor(x => x.Email)
				.NotEqual("Invalid")
				.WithMessage("Invalid email")
				.WithName("email");
		}
	}

	public class CreateProductInput
	{
		public string Name { get; set; }
	}

	public class CreateProductInputType : InputObjectType<CreateProductInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<CreateProductInput> descriptor)
		{
			descriptor.Field(x => x.Name);
		}
	}

	public class CreateProductInputValidator : AbstractValidator<CreateProductInput>
	{
		public CreateProductInputValidator()
		{
			RuleFor(x => x.Name)
				.NotEqual("Invalid")
				.WithMessage("Invalid product name")
				.WithName("name");
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

		public CreateProductPayload CreateProduct(CreateProductInput input)
		{
			return new()
			{
				Id = Guid.NewGuid().ToString("N"),
				Name = input.Name
			};
		}
	}

	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IValidator<CreateUserInput>, CreateUserInputValidator>();
			services.AddTransient<IValidator<CreateProductInput>, CreateProductInputValidator>();

			services.AddGraphQLServer()
				.AddFluentValidation()
				.AddQueryType(x => x.Name("Query").Field("test").Resolve("Works!"))
				.AddMutationType(x =>
				{
					var mutation = x.Name("Mutation");

					mutation.Field<UserResolvers>(r => r.CreateUser(default!))
						.Type(new ValidationResultType<CreateUserPayloadType>("CreateUser", new Dictionary<string, string[]>
						{
							["input"] = new[] { "userName", "email" }
						}))
						.Argument("input", arg => arg.Type<CreateUserInputType>().UseFluentValidation());

					mutation.Field<UserResolvers>(r => r.CreateProduct(default!))
						.Type(new ValidationResultType<CreateProductPayloadType>("CreateProduct", new Dictionary<string, string[]>
						{
							["input"] = new[] { "name" }
						}))
						.Argument("input", arg => arg.Type<CreateProductInputType>().UseFluentValidation());
				});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGraphQL();
			});
		}
	}
}
