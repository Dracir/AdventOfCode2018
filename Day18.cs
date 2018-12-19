using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2018
{
	public class Day18
	{

		public static void Work()
		{


			string[] test = Parse(File.ReadAllLines("Data/D18Test.txt"));
			string[] input = File.ReadAllLines("Data/D18Input.txt");


			/* Asset.AreEqual(1147, Part1(test, 10), "Part1 Test 1");
			Asset.AreEqual(675100, Part1(input, 10, false), "Part1 Input Answer");
			//NoelConsole.WriteWithTime(() => "" + Part1(input,10));
			NoelConsole.WriteWithTime(() => "" + Part1(input, 1000000000, false)); */

			//NoelConsole.WriteWithTime(() => "" + Part2(File.ReadAllLines("Data/D18Test.txt"), 10, true));
			//NoelConsole.WriteWithTime(() => "" + Part2(input, 10, true));
			Asset.AreEqual(675100, Part2(input, 10, false), "Part1 Input Answer");
			Asset.AreEqual(191820, Part2(input, 1000000000, false), "Part2 Input Answer");
			//NoelConsole.WriteWithTime(() => "" + Part2(input, 1000000000, false));
		}

		private static string[] Parse(string[] v)
		{
			return v
			.Prepend(new string(' ', v[0].Length))
			.Append(new string(' ', v[0].Length))
			.Select(x => x.Append(' ').Prepend(' '))
			.Select(x => new String(x.ToArray()))
			.ToArray();
		}


		private static int Part1(string[] input, long nbSimulation, bool print = false)
		{
			var area = input;
			int value = NoelConsole.WritingYPosition + 1;

			if (print)
			{
				NoelConsole.WritingYPosition = value;
				NoelConsole.Write(String.Join('\n', area));
			}


			for (long i = 0; i < nbSimulation; i++)
			{
				area = SimulateWide(area);
				if (print)
				{
					NoelConsole.WritingYPosition = value;
					NoelConsole.Write(String.Join('\n', area));
					Thread.Sleep(100);
				}

			}
			return area.Sum(x => x.Count(y => y == '#')) * area.Sum(x => x.Count(y => y == '|'));
		}

		private static string[] SimulateWide(string[] area)
		{
			var output = new string[area.Length];
			var emptyline = new String(' ', area[0].Length);
			output[0] = emptyline;
			output[area.Length - 1] = emptyline;

			return Enumerable
			.Range(1, area.Length - 2)
			.AsParallel()
			.AsOrdered()
			.Select(line => DoALineWide(area, line))
			.Prepend(new string(' ', area[0].Length))
			.Append(new string(' ', area[0].Length))
			.ToArray();

		}

		private static string DoALineWide(string[] area, int lineIndex)
		{
			var line = new char[area[0].Length];
			line[0] = ' ';
			line[area[0].Length - 1] = ' ';

			for (int j = 1; j < area[1].Length - 1; j++)
			{
				int nbTreesAdjacent = CountWide('|', area, lineIndex, j);
				int nbLumberAdjacent = CountWide('#', area, lineIndex, j);

				if (area[lineIndex][j] == '.')
				{
					if (nbTreesAdjacent >= 3)
						line[j] = '|';
					else
						line[j] = '.';
				}
				else if (area[lineIndex][j] == '|')
				{
					if (nbLumberAdjacent >= 3)
						line[j] = '#';
					else
						line[j] = '|';
				}
				else
				{
					if (nbLumberAdjacent >= 1 && nbTreesAdjacent >= 1)
						line[j] = '#';
					else
						line[j] = '.';
				}
			}

			return new String(line);
		}

		private static int CountWide(char v, string[] area, int i, int j)
		{
			int count = 0;

			if (area[i - 1][j - 1] == v) count++;
			if (area[i][j - 1] == v) count++;
			if (area[i + 1][j - 1] == v) count++;
			if (area[i - 1][j] == v) count++;
			if (area[i + 1][j] == v) count++;
			if (area[i - 1][j + 1] == v) count++;
			if (area[i][j + 1] == v) count++;
			if (area[i + 1][j + 1] == v) count++;

			return count;
		}

		private static int Part2(string[] input, long nbSimulation, bool print = false)
		{

			//NoelConsole.Write("Generating All Map Posiblities ...");

			int value = NoelConsole.WritingYPosition + 1;
			if (print)
			{
				NoelConsole.WritingYPosition = value;
				NoelConsole.Write(String.Join('\n', input));
			}

			var maps = new List<string[]>();
			var mapsLookup = new Dictionary<string[], int>(new MapEqualityComparer());
			var transition = new Dictionary<int, int>();


			maps.Add(input);
			mapsLookup.Add(input, 0);
			var mapEmpty = MakeEmptyMap(input.Length);

			int currentIndex = 0;
			int firstIndexLoop = 0;
			int loopbackIndex = 0;
			for (int i = 1; i <= nbSimulation; i++)
			{
				if (currentIndex >= transition.Count)
				{
					var newMap = Simulate(maps[currentIndex]);
					int nextValue = 0;
					if (mapsLookup.TryGetValue(newMap, out nextValue))
					{
						if (firstIndexLoop == 0)
						{
							loopbackIndex = transition[nextValue];
							firstIndexLoop = i;
							//currentIndex = loopbackIndex + (int)((nbSimulation - i) % (loopbackIndex - firstIndexLoop))-1;
							//break;
						}
					}
					else
					{
						maps.Add(newMap);
						mapsLookup.Add(newMap, maps.Count - 1);
						nextValue = maps.Count - 1;
					}

					transition.Add(currentIndex, nextValue);
				}
				currentIndex = transition[currentIndex];

				if (print)
				{
					NoelConsole.WritingYPosition = value;
					NoelConsole.Write(String.Join('\n', maps[currentIndex]));
					Thread.Sleep(100);
				}
			}

			var lastArea = maps[currentIndex];
			NoelConsole.Write("Nb Diff Maps :" + maps.Count);
			NoelConsole.Write("Nb Transition :" + transition.Count);
			NoelConsole.Write("Loop Back Index :" + loopbackIndex);
			NoelConsole.Write("Loop size :" + (firstIndexLoop-loopbackIndex));
			NoelConsole.Write("First Index start Loop :" + firstIndexLoop);
			return lastArea.Sum(x => x.Count(y => y == '#')) * lastArea.Sum(x => x.Count(y => y == '|'));
			/* Enumerable.Range(1, 50 * 3)
			.AsParallel()
			.SelectMany(x => MakeMap(x));
			return 1; */

		}

		private class MapEqualityComparer : IEqualityComparer<String[]>
		{
			public bool Equals(String[] map1, String[] map2)
			{
				for (int i = 0; i < map1.Length; i++)
				{
					if (map1[i] != map2[i])
						return false;
				}
				return true;
			}

			public int GetHashCode(String[] bx)
			{
				return bx.GetHashCode();
			}
		}



		private static string[] Simulate(string[] area)
		{
			var output = new string[area.Length];
			var emptyline = new String(' ', area[0].Length);

			return Enumerable
			.Range(0, area.Length)
			.AsParallel()
			.AsOrdered()
			.Select(line => DoALine(area, line))
			.ToArray();
		}

		private static string DoALine(string[] area, int lineIndex)
		{
			var line = new char[area[0].Length];

			for (int j = 0; j < area[1].Length; j++)
			{
				int nbTreesAdjacent = Count('|', area, lineIndex, j);
				int nbLumberAdjacent = Count('#', area, lineIndex, j);

				if (area[lineIndex][j] == '.')
				{
					if (nbTreesAdjacent >= 3)
						line[j] = '|';
					else
						line[j] = '.';
				}
				else if (area[lineIndex][j] == '|')
				{
					if (nbLumberAdjacent >= 3)
						line[j] = '#';
					else
						line[j] = '|';
				}
				else
				{
					if (nbLumberAdjacent >= 1 && nbTreesAdjacent >= 1)
						line[j] = '#';
					else
						line[j] = '.';
				}
			}

			return new String(line);
		}

		private static int Count(char v, string[] area, int i, int j)
		{
			int count = 0;
			int size = area.Length - 1;

			if (i > 0 && j > 0 && area[i - 1][j - 1] == v) count++;
			if (j > 0 && area[i][j - 1] == v) count++;
			if (j > 0 && i < size && area[i + 1][j - 1] == v) count++;
			if (i > 0 && area[i - 1][j] == v) count++;
			if (i < size && area[i + 1][j] == v) count++;
			if (i > 0 && j < size && area[i - 1][j + 1] == v) count++;
			if (j < size && area[i][j + 1] == v) count++;
			if (i < size && j < size && area[i + 1][j + 1] == v) count++;

			return count;

		}




		private static String[] MakeEmptyMap(int size)
		{
			var map = new String[size];
			for (int y = 0; y < size; y++)
			{
				var line = new char[size];
				for (int x = 0; x < size; x++)
				{
					line[x] = '.';
				}
				map[y] = new String(line);
			}
			return map;
		}
	}
}