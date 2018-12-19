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
			string[] input = Parse(File.ReadAllLines("Data/D18Input.txt"));


			/* Asset.AreEqual(1147, Part1(test, 10), "Part1 Test 1");
			Asset.AreEqual(675100, Part1(input, 10, false), "Part1 Input Answer");
			//NoelConsole.WriteWithTime(() => "" + Part1(input,10));
			NoelConsole.WriteWithTime(() => "" + Part1(input, 1000000000, false)); */
			
			NoelConsole.WriteWithTime(() => "" + Part2(input,10));

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
				area = Simulate(area);
				if (print)
				{
					NoelConsole.WritingYPosition = value;
					NoelConsole.Write(String.Join('\n', area));
					Thread.Sleep(100);
				}

			}
			return area.Sum(x => x.Count(y => y == '#')) * area.Sum(x => x.Count(y => y == '|'));
		}

		private static string[] Simulate(string[] area)
		{
			var output = new string[area.Length];
			var emptyline = new String(' ', area[0].Length);
			output[0] = emptyline;
			output[area.Length - 1] = emptyline;

			return Enumerable
			.Range(1, area.Length - 2)
			.AsParallel()
			.AsOrdered()
			.Select(line => DoALine(area, line))
			.Prepend(new string(' ', area[0].Length))
			.Append(new string(' ', area[0].Length))
			.ToArray();

		}

		private static string DoALine(string[] area, int lineIndex)
		{
			var line = new char[area[0].Length];
			line[0] = ' ';
			line[area[0].Length - 1] = ' ';

			for (int j = 1; j < area[1].Length - 1; j++)
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
			
			NoelConsole.Write("Generating All Map Posiblities ...");

			var maps = new Dictionary<int,string[]>();
			Enumerable.Range(1,50*3)
			.AsParallel()
			.Select(x=>MakeMap(x));
			return 1;

		}

		private static string[] MakeMap(int tile)
		{
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					
				}
			}
		}
	}
}