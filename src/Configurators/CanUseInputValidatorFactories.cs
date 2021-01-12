namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseInputValidatorFactories<TConfigurator>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="InputValidatorFactory"/>
		/// </summary>
		TConfigurator UseInputValidatorFactories(params InputValidatorFactory[] inputValidatorFactories);
	}
}
