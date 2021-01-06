using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Maps validation result to <see cref="IError"/> properties
	/// </summary>
	/// <param name="errorBuilder"></param>
	/// <param name="mappingContext"></param>
	public delegate void ErrorMapper(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext);
}
