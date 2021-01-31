using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class ErrorMappersBenchmarks
	{
		private IRequestExecutor withDefaultErrorMapper = default!;
		private IRequestExecutor withDefaultErrorMapperWithDetails = default!;
		private IRequestExecutor withDefaultErrorMapperWithExtendedDetails = default!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withDefaultErrorMapper = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field =>
						field.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());

			withDefaultErrorMapperWithDetails = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithDetails())
					.AddMutationType(new TestMutationType(field =>
						field.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());

			withDefaultErrorMapperWithExtendedDetails = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation(opt => opt.UseDefaultErrorMapperWithExtendedDetails())
					.AddMutationType(new TestMutationType(field =>
						field.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());
		}

		[Benchmark(Baseline = true)]
		public Task RunWithDefaultErrorMapper()
		{
			return withDefaultErrorMapper.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithDefaultErrorMapperWithDetails()
		{
			return withDefaultErrorMapperWithDetails.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithDefaultErrorMapperWithExtendedDetails()
		{
			return withDefaultErrorMapperWithExtendedDetails.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}
	}
}
