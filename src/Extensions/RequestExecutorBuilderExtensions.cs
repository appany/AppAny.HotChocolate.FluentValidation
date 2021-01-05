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
			Action<IFluentValidationConfigurator> configure)
		{
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new FluentValidationConfigurator(services);

				configurator.UseErrorMappers(
					ValidationDefaults.ErrorMappers.Default,
					ValidationDefaults.ErrorMappers.Extensions);

				configurator.UseValidatorFactories(ValidationDefaults.ValidationFactories.Default);

				configure.Invoke(configurator);
			});

			builder.ConfigureSchema(schemaBuilder =>
			{
				schemaBuilder.Use<FluentValidationMiddleware>();
			});

			return builder;
		}
	}
}
