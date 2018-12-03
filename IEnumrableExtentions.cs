using System;
using System.Collections.Generic;

namespace AdventOfCode2018
{
	public static class IEnumrableExtentions
	{
		public static TAccumulate AggregateUntil<TSource, TAccumulate>(
			   this IEnumerable<TSource> source, TAccumulate seed,
			   Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, bool> predicate)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (func == null)
				throw new ArgumentNullException(nameof(func));

			if (predicate == null)
				throw new ArgumentNullException(nameof(func));

			var accumulate = seed;
			foreach (var item in source)
			{
				accumulate = func(accumulate, item);
				if (predicate(accumulate)) break;
			}
			return accumulate;
		}
	}
}