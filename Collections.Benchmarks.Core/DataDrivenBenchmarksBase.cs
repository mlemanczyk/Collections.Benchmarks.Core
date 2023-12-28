using BenchmarkDotNet.Attributes;
using Collections.Pooled;
using Recyclable.Collections;

namespace Collections.Benchmarks.Core
{
	[MemoryDiagnoser]
	public class DataDrivenBenchmarksBase<TTestCase> : BaselineVsActualBenchmarkBase<TTestCase>
	{
		protected int[]? _testObjects;
		protected int[] TestObjects => _testObjects ?? throw new NullReferenceException("Something is wrong and the field is not initialized");
		protected List<int>? _testObjectsAsList;
		protected List<int> TestObjectsAsList => _testObjectsAsList ?? throw new NullReferenceException("Something went wrong and the field is not initialized");
		protected PooledList<int>? _testObjectsAsPooledList;
		public PooledList<int> TestObjectsAsPooledList => _testObjectsAsPooledList ?? throw new NullReferenceException("Something is wrong and the field is not initialized");
		protected RecyclableList<int>? _testObjectsAsRecyclableList;
		protected RecyclableList<int> TestObjectsAsRecyclableList => _testObjectsAsRecyclableList ?? throw new NullReferenceException("Something is wrong and the field is not initialized");
		protected RecyclableLongList<int>? _testObjectsAsRecyclableLongList;
		protected RecyclableLongList<int> TestObjectsAsRecyclableLongList => _testObjectsAsRecyclableLongList ?? throw new NullReferenceException("Something is wrong and the field is not initialized");
		protected IEnumerable<int> TestObjectsAsIEnumerable
		{
			get
			{
				for (int i = 1; i <= TestObjectCount; i++)
				{
					yield return i;
				}
			}
		}

		protected override string GetTestMethodName(TTestCase? benchmarkType)
		{
			return $"{base.GetTestMethodName(benchmarkType)}{MethodNameSuffix}";
		}

		protected override void PrepareData<T>(T benchmarkType)
		{
			Console.WriteLine($"******* PREPARING DATA FOR {benchmarkType} *******");
			switch (benchmarkType)
			{
				case CollectionsBenchmarksSource.Unknown:
					break;

				case CollectionsBenchmarksSource.Array:
					_testObjects ??= DataGenerator.EnumerateTestObjects(TestObjectCount);
					break;

				case CollectionsBenchmarksSource.List:
					_testObjectsAsList ??= TestObjects.ToList();
					break;

				case CollectionsBenchmarksSource.PooledList:
					_testObjectsAsPooledList ??= new(TestObjects, ClearMode.Auto);
					break;

				case CollectionsBenchmarksSource.RecyclableList:
					_testObjectsAsRecyclableList ??= new(TestObjects, initialCapacity: TestObjectCount);
					break;

				case CollectionsBenchmarksSource.RecyclableLongList:
					_testObjectsAsRecyclableLongList ??= new(TestObjects, BlockSize, initialCapacity: base.TestObjectCount);
					break;

				default:
					throw CreateUnknownBenchmarkTypeException(benchmarkType);
			}

			base.PrepareData(benchmarkType);
		}
		
		public override void Cleanup()
		{
			Console.WriteLine("******* GLOBAL CLEANUP *******");

			if (_testObjects?.Length > 0 && !_testObjects[0].GetType().IsValueType)
			{
				Array.Fill(_testObjects, 0);
			}

			_testObjects = default;
			_testObjectsAsList = default;
			_testObjectsAsPooledList?.Dispose();
			_testObjectsAsPooledList = default;
			_testObjectsAsRecyclableList?.Dispose();
			_testObjectsAsRecyclableList = default;
			_testObjectsAsRecyclableLongList?.Dispose();
			_testObjectsAsRecyclableLongList = default;
			base.Cleanup();
		}

		public override void Setup()
		{
			PrepareData(CollectionsBenchmarksSource.Array);
			base.Setup();
		}

		public virtual string MethodNameSuffix { get; } = string.Empty;
	}
}
