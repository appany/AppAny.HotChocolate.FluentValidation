using System;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class RequestExecutorBuilderExtensions
	{
		public static IRequestExecutorBuilder AddFluentValidation(this IRequestExecutorBuilder builder)
		{
			return builder.AddFluentValidation(_ =>
			{
			});
		}

		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<IInputValidationConfigurator> configure)
		{
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new InputValidationConfigurator(services);

				configurator.AddDefaultErrorMapperWithDetails();
				configurator.AddDefaultValidatorFactory();

				configure.Invoke(configurator);
			});

			builder.ConfigureSchema(schemaBuilder =>
			{
				schemaBuilder.Use<InputValidationMiddleware>();
			});

			return builder;
		}
	}
}
