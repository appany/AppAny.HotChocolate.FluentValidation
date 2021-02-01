using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentValidation;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class InputValidatorBenchmarks
	{
		private IValidator validator = default!;
		private InputValidator inputValidator = default!;

		[GlobalSetup]
		public void GlobalSetup()
		{
			validator = new TestInputValidator();
			inputValidator = ValidationDefaults.InputValidators.FromValidator(validator);
		}

		[Benchmark(Baseline = true)]
		public async Task Validation()
		{
			var validationContext = new ValidationContext<TestInput>(new TestInput());

			await validator.ValidateAsync(validationContext, CancellationToken.None);
		}

		[Benchmark]
		public async Task InputValidatorValidation()
		{
			await inputValidator.Invoke(new InputValidatorContext(new TestInput(), CancellationToken.None));
		}
	}
}
