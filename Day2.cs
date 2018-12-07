using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AdventOfCode2018
{
	public class Day2
	{
		public static void Work()
		{

			string[] test1 = File.ReadAllLines("Data/D2Test1.txt");
			string[] input = File.ReadAllLines("Data/D2Input.txt");

			NoelConsole.Write("*Day 2 - Part 1*");

			Asset.AreEqual(12, Part1(test1), "Test1");

			NoelConsole.WriteWithTime(() => "" + Part1(input));


			NoelConsole.Write("\n*Day  - Part 2*");
			NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static int Part1(string[] input)
		{
			var charCounts = input.Select(value => CountDupplicate(value));

			return new int[] { 2, 3 }.Product(value => charCounts.Where(dic => dic.Any(kv => kv.Value == value)).Count());
		}

		private static Dictionary<char, int> CountDupplicate(string value) =>
			value.Aggregate(new Dictionary<char, int>(), (dic, character) => dic.AddValue(character, 1));


		private static string Part2(string[] input)
		{

			var a = GetKeyValues(input).First(kv => HasOnly1Different(kv.Key, kv.Value));
			return GetCharChar(a.Key,a.Value).Aggregate("",(str,kv) =>  str += (kv.Key == kv.Value) ? kv.Value.ToString() : "");
		}

		private static bool HasOnly1Different(string key, string value) =>
		GetCharChar(key, value).Count(a => a.Value != a.Key) == 1;

		private static IEnumerable<KeyValuePair<char, char>> GetCharChar(string a, string b)
		{
			for (int i = 0; i < a.Length; i++)
				yield return new KeyValuePair<char, char>(a[i], b[i]);

		}

		private static IEnumerable<KeyValuePair<string, string>> GetKeyValues(string[] values)
		{
			for (int i = 0; i < values.Length; i++)
				for (int j = i + 1; j < values.Length; j++)
					yield return new KeyValuePair<string, string>(values[i], values[j]);
		}



	}
}