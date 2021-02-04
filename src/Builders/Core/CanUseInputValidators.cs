namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseInputValidators<TBuilder>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="InputValidator"/>
		/// </summary>
		TBuilder UseInputValidators(params InputValidator[] inputValidators);
	}
}
