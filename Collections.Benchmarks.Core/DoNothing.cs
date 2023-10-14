using System.Runtime.CompilerServices;

#pragma warning disable IDE0060 // Remove unused parameter

namespace Collections.Benchmarks.Core
{
	public static class DoNothing
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void With<T>(in T item)
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void With<T>(in T item1, in T item2)
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void With<T>(ReadOnlySpan<T> span)
		{
		}
	}
}
