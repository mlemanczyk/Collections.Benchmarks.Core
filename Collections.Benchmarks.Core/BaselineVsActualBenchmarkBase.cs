using BenchmarkDotNet.Attributes;
using System.Numerics;
using System.Reflection;

namespace Collections.Benchmarks.Core
{
	public abstract class BaselineVsActualBenchmarkBase<TBenchmarkType>
	{
		public virtual TBenchmarkType? DataType { get; set; }
		public virtual TBenchmarkType? BaseDataType { get; set; }

		private Action? _testMethod;
		protected Action TestMethod
		{
			get => _testMethod ?? throw new OperationCanceledException("Skip - No actual defined");
			set => _testMethod = value;
		}

		private Action? _baselineMethod;
		protected Action BaselineMethod
		{
			get => _baselineMethod ?? throw new OperationCanceledException("Skip - No baseline defined");
			set => _baselineMethod = value;
		}

		[Benchmark(Baseline = true)]
		public void Baseline() => BaselineMethod.Invoke();

		[Benchmark]
		public void Actual() => TestMethod.Invoke();

		public virtual int BlockSize { get; protected set; }
		public byte BlockSizePow2BitShift => checked((byte)(31 - BitOperations.LeadingZeroCount((uint)BlockSize)));
		public virtual int Divider { get; protected set; } = 10;
		public virtual int TestObjectCount { get; set; } = 0;
		public virtual int TestObjectCountForSlowMethods { get; private set; }

		protected static Exception CreateUnknownBenchmarkTypeException<T>(T benchmarkType) => new InvalidOperationException($"******* UNKNOWN BENCHMARK TYPE {{{benchmarkType}}} *******");
		protected static Exception CreateMethodNotFoundException(in string methodName, in string? className) => new MethodAccessException($"******* METHOD {{{methodName}}} NOT FOUND IN CLASS {{{className}}} *******");

		protected virtual void PrepareData<T>(T benchmarkType) { }
		protected virtual Action? GetTestMethod(TBenchmarkType? benchmarkType)
			=> (Action?)GetType().GetMethod(GetTestMethodName(benchmarkType), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)?.CreateDelegate(typeof(Action), this) ?? throw CreateMethodNotFoundException(GetTestMethodName(benchmarkType), GetType().FullName);

		protected virtual string GetTestMethodName(TBenchmarkType? benchmarkType) => $"{benchmarkType}";

		[GlobalCleanup]
		public virtual void Cleanup()
		{
			_baselineMethod = null;
			_testMethod = null;
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
		}

		[GlobalSetup]
		public virtual void Setup()
		{
			Console.WriteLine($"******* SETTING UP TEST CASE FOR BENCHMARK {{{DataType}}} *******");
			
			TestObjectCountForSlowMethods = (TestObjectCount / 2) + 1;
			
			BlockSize = TestObjectCount switch
			{
				0 => 1,
				> 0 and <= 10_240 => TestObjectCount,
				_ => TestObjectCount / Divider > 0 ? TestObjectCount / Divider : TestObjectCount
			};

			if (BlockSize > 0)
			{
				BlockSize = checked((int)BitOperations.RoundUpToPowerOf2((uint)BlockSize));
			}
			
			Console.WriteLine($"******* BaseDataType = {BaseDataType}; default = {default(TBenchmarkType)} *******");

			if (!EqualityComparer<TBenchmarkType>.Default.Equals(BaseDataType, default))
			{
				Console.WriteLine("******* SETTING UP BASELINE DATA *******");
				PrepareData(BaseDataType);
				_baselineMethod = GetTestMethod(BaseDataType);
			}

			//~ We don't want to prepare data if baseline data is the same is benchmark data.
			//~ One would override another - waste of time & resources.
			if (!EqualityComparer<TBenchmarkType>.Default.Equals(BaseDataType, DataType))
			{
				Console.WriteLine("******* SETTING UP TEST CASE DATA *******");
				//~ If BaseDataType == null, then we'll come here only if DataType != null.
				PrepareData(DataType!);
				_testMethod = GetTestMethod(DataType);
			}

			Console.WriteLine("******* DATA PREPARED *******");
		}
	}
}
