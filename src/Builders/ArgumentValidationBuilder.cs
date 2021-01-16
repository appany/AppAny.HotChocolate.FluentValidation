using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface ArgumentValidationBuilder
		: CanSkipValidation<ArgumentValidationBuilder>,
			CanUseInputValidatorProviders<ArgumentValidationBuilder>,
			CanUseErrorMappers<ArgumentValidationBuilder>
	{
	}

	internal sealed class DefaultArgumentValidationBuilder : ArgumentValidationBuilder
	{
		private readonly ArgumentValidationOptions options;

		public DefaultArgumentValidationBuilder(ArgumentValidationOptions options)
		{
			this.options = options;
		}

		public ArgumentValidationBuilder SkipValidation(SkipValidation skipValidation)
		{
			options.SkipValidation = skipValidation;

			return this;
		}

		public ArgumentValidationBuilder UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders)
		{
			if (options.InputValidatorProviders is null)
			{
				options.InputValidatorProviders = inputValidatorProviders.ToList();
			}
			else
			{
				foreach (var inputValidatorProvider in inputValidatorProviders)
				{
					options.InputValidatorProviders.Add(inputValidatorProvider);
				}
			}

			return this;
		}

		public ArgumentValidationBuilder UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			if (options.ErrorMappers is null)
			{
				options.ErrorMappers = errorMappers.ToList();
			}
			else
			{
				foreach (var errorMapper in errorMappers)
				{
					options.ErrorMappers.Add(errorMapper);
				}
			}

			return this;
		}
	}
}
