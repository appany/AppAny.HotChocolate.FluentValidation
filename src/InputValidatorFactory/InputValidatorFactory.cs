namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Resolves <see cref="InputValidator"/> by specified convention
	/// </summary>
	public delegate InputValidator InputValidatorFactory(
		InputValidatorFactoryContext inputValidatorFactoryContext);
}
