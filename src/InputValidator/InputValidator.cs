using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Abstracts <see cref="IValidator"/> execution
	/// </summary>
	public delegate Task<ValidationResult> InputValidator(object argument, CancellationToken cancellationToken);
}
