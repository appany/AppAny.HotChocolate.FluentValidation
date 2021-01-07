using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	public delegate Task<ValidationResult> InputValidator(object argument, CancellationToken cancellationToken);
}
