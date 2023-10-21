namespace Collections.Benchmarks.Core
{
	public enum CollectionsBenchmarkType
	{
		Unknown, Add_WithCapacity, Add, AddRange_WithCapacity, AddRangeWhenSourceIsArray, AddRangeWhenSourceIsIEnumerable,
		AddRangeWhenSourceIsIList, AddRangeWhenSourceIsList, AddRangeWhenSourceIsSameType, BinarySearch_BestAndWorstCases, 
		Contains_FirstItems, Contains_LastItems, ConvertAll, Count, Create_WithCapacity, Create, Exists_BestAndWorstCases,
		Find_BestAndWorstCases, FindAll_BestAndWorstCases, FindLast_BestAndWorstCases, FindLastIndex_BestAndWorstCases,
		ForEach, GetItem, IndexOf_BestAndWorstCases,IndexOf_FirstItems, IndexOf_LastItems, LongCount,
		Remove_FirstItems, Remove_LastItems, RemoveAt_FirstItems, RemoveAt_LastItems,
		SetItem, VersionedForEach
	}
}
