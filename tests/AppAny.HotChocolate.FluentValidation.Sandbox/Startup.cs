using System;
using FluentValidation;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Sandbox
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class CreateUserPayload
	{
		public User User1 { get; set; }
		public User User2 { get; set; }
	}

	public class CreateUserPayload2
	{
		public User User1 { get; set; }
	}

	public class CreateUserPayloadType : ObjectType<CreateUserPayload>
	{
		protected override void Configure(IObjectTypeDescriptor<CreateUserPayload> descriptor)
		{
			descriptor.Field(x => x.User1);
			descriptor.Field(x => x.User2);
		}
	}

	public class CreateUserPayload2Type : ObjectType<CreateUserPayload2>
	{
		protected override void Configure(IObjectTypeDescriptor<CreateUserPayload2> descriptor)
		{
			descriptor.Field(x => x.User1);
		}
	}


	public class UserType : ObjectType<User>
	{
		protected override void Configure(IObjectTypeDescriptor<User> descriptor)
		{
			descriptor.Field(x => x.Id);
			descriptor.Field(x => x.Name);
		}
	}

	public class CreateUserInputValidator : AbstractValidator<CreateUserInput>
	{
		public CreateUserInputValidator()
		{
			RuleFor(x => x.Name)
				.NotEqual("Invalid")
				.WithMessage("Name is invalid");
		}
	}

	public class CreateAccountInputValidator : AbstractValidator<CreateAccountInput>
	{
		public CreateAccountInputValidator()
		{
			RuleFor(x => x.Id)
				.NotEqual(13)
				.WithMessage("Id is invalid");
		}
	}

	public class CreateUserInput
	{
		public string Name { get; set; }
	}

	public class CreateAccountInput
	{
		public int Id { get; set; }
	}

	public class CreateUserInputType : InputObjectType<CreateUserInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<CreateUserInput> descriptor)
		{
			descriptor.Field(x => x.Name);
		}
	}

	public class CreateAccountInputType : InputObjectType<CreateAccountInput>
	{
		protected override void Configure(IInputObjectTypeDescriptor<CreateAccountInput> descriptor)
		{
			descriptor.Field(x => x.Id);
		}
	}

	public class MutationType : ObjectType
	{
		protected override void Configure(IObjectTypeDescriptor descriptor)
		{
			descriptor.Name("Mutation");

			descriptor.Field<MutationType>(x => x.CreateUser(default!, default!))
				.Type<CreateUserPayloadType>()
				.Argument("input", arg => arg.Type<CreateUserInputType>().UseFluentValidation())
				.Argument("input2", arg => arg.Type<CreateUserInputType>().UseFluentValidation());

			descriptor.Field<MutationType>(x => x.CreateUser2(default!))
				.Type<CreateUserPayload2Type>()
				.Argument("input322", arg => arg.Type<CreateAccountInputType>().UseFluentValidation());
		}

		public CreateUserPayload CreateUser(CreateUserInput input, CreateUserInput input2)
		{
			var user1 = new User
			{
				Id = new Random().Next(0, 1000),
				Name = input.Name
			};

			var user2 = new User
			{
				Id = 123321,
				Name = input2.Name
			};

			return new CreateUserPayload
			{
				User1 = user1,
				User2 = user2
			};
		}

		public CreateUserPayload2 CreateUser2(CreateAccountInput input322)
		{
			var user1 = new User
			{
				Id = input322.Id,
				Name = "input322."
			};

			return new CreateUserPayload2
			{
				User1 = user1
			};
		}
	}

	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGraphQLServer()
				.InitializeOnStartup()
				.AddFluentValidation()
				.AddQueryType(x => x.Name("Query").Field("test").Resolve("works"))
				.AddMutationType<MutationType>();

			services.AddTransient<IValidator<CreateUserInput>, CreateUserInputValidator>();
			services.AddTransient<IValidator<CreateAccountInput>, CreateAccountInputValidator>();
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
