using System;
using System.Collections.Generic;
using System.Linq;

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

		public static int Product<T>(this IEnumerable<T> enumerable, Func<T, int> func)
		{
			return enumerable.Aggregate(1, (mult, value) => mult * func(value));
		}

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}


		public static T MaxBy<T>(this IEnumerable<T> enumeration, Func<T, int> selector)
		{
			return enumeration.Aggregate(enumeration.First(), (currMax, x) => selector(currMax) < selector(x) ? x : currMax);
		}

		public static (T,int) FirstBy<T>(this IEnumerable<T> enumeration, Func<T, bool> selector)
		{
			return enumeration.Select((x,i)=>(x,i)).First(x=>selector(x.x));

		}



		public static string JoinStr<T>(this IEnumerable<T> enumeration, Func<T, string> selector, char separator)
		{
			return enumeration.Aggregate("", (agg, x) => (agg.Count() == 0) ? selector(x) : (agg + "|" + selector(x)));

		}
		public static string JoinStr(this IEnumerable<string> enumeration, char separator)
		{
			return enumeration.Aggregate("", (agg, x) => (agg.Count() == 0) ? x : (agg + "|" + x));

		}

		public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func <T,IEnumerable<T>> selector)
		{
			return e.SelectMany(c => Flatten(selector(c), selector));
		}


		public static List<Tuple<T, T>> PairUp<T>(this IEnumerable<T> enumeration)
		{
			return enumeration.Select((item, index) => new { item, index }).GroupBy(c => c.index - c.index % 2).Select(group => Tuple.Create<T, T>(group.First().item, group.ToArray()[1].item)).ToList();
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static IEnumerable<char> AllCapsLetters()
		{
			for (int i = 0; i < 24; i++)
			{
				yield return (char)('A' + i);
			}
		}
	}
}