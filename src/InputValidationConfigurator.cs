using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures global validation options
	/// </summary>
	public interface IInputValidationConfigurator
	{
		/// <summary>
		/// Overrides default <see cref="ErrorMapper"/>
		/// </summary>
		IInputValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);

		/// <summary>
		/// Overrides default <see cref="InputValidatorFactory"/>
		/// </summary>
		IInputValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] validatorFactories);
	}

	internal sealed class InputValidationConfigurator : IInputValidationConfigurator
	{
		private readonly IServiceCollection services;

		public InputValidationConfigurator(IServiceCollection services)
		{
			this.services = services;
		}

		public IInputValidationConfigurator SkipValidation(SkipValidation skipValidation)
		{
			services.Configure<InputValidationOptions>(options => options.SkipValidation = skipValidation);

			return this;
		}

		public IInputValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			services.Configure<InputValidationOptions>(options => options.ErrorMappers = errorMappers);

			return this;
		}

		public IInputValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] validatorFactories)
		{
			services.Configure<InputValidationOptions>(options => options.ValidatorFactories = validatorFactories);

			return this;
		}
	}
}
