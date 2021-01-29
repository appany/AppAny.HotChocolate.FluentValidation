namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanUseErrorMappers<TBuilder>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="ErrorMapper"/>
		/// </summary>
		TBuilder UseErrorMappers(params ErrorMapper[] errorMappers);
	}
}
