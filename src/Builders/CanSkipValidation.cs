namespace AppAny.HotChocolate.FluentValidation
{
	public interface CanSkipValidation<TConfigurator>
	{
		/// <summary>
		/// Implementation specific. Can add or override <see cref="SkipValidation"/>
		/// </summary>
		TConfigurator SkipValidation(SkipValidation skipValidation);
	}
}
