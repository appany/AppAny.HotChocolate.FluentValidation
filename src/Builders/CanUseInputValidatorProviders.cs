namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseInputValidatorProviders<TConfigurator>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="InputValidatorProvider"/>
		/// </summary>
		TConfigurator UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders);
	}
}
