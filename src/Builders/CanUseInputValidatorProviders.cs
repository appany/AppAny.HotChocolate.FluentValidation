namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseInputValidatorProviders<TBuilder>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="InputValidatorProvider"/>
		/// </summary>
		TBuilder UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders);
	}
}
