using System.Threading;

namespace AppAny.HotChocolate.FluentValidation
{
	public readonly struct InputValidatorContext
	{
		public InputValidatorContext(object argument, CancellationToken cancellationToken)
		{
			Argument = argument;
			CancellationToken = cancellationToken;
		}

		public object Argument { get; }
		public CancellationToken CancellationToken { get; }
	}
}
