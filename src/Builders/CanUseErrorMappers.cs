namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseErrorMappers<TConfigurator>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="ErrorMapper"/>
		/// </summary>
		TConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
	}
}
