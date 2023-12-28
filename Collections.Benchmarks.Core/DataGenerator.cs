namespace Collections.Benchmarks.Core
{
	public static class DataGenerator
	{
		public static int[] EnumerateTestObjects(int testObjectCount)
		{
			var result = new int[testObjectCount];
			Console.WriteLine("******* TEST DATA ARRAY CREATED *******");

			Span<int> resultSpan = result;
			for (int i = 0; i < testObjectCount; i++)
			{
				resultSpan[i] = i;
			}

			return result;
		}

		public static IEnumerable<long> EnumerateTestObjects(long testObjectCount)
		{
			Console.WriteLine("******* TEST DATA ARRAY CREATED *******");

			for (int i = 1; i <= testObjectCount; i++)
			{
				yield return i;
			}
		}
	}
}