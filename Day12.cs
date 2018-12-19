using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day12
	{
		public static void Work()
		{


			var inputParsed = Parse(File.ReadAllLines("Data/D12Input.txt"));
			var inpoutMath = Parse(File.ReadAllLines("Data/D12Math.txt"));
			NoelConsole.WriteWithTime(() => "" + Part1(inpoutMath,20,false));
			//NoelConsole.WriteWithTime(() => "" + Part1(inputParsed, 50000000000, true));

			//Asset.AreEqual(138, Part1(test), "Part1 Test 1");
			//NoelConsole.WriteWithTime(() => "" + Part1(input));
			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static (bool[], Dictionary<int, bool>) Parse(string[] v)
		{
			var spread = new Dictionary<int, bool>();

			var init = v[0].Skip(15).Select(x => x == '#').Append(false).Append(false).Append(false).Append(false).Prepend(false).Prepend(false).Prepend(false).Prepend(false).ToArray();

			for (int i = 2; i < v.Length; i++)
			{

				var keyBools = v[i].SkipLast(5).Select(x => x == '#').ToArray();
				var key = BoolArrToIn(keyBools);
				var value = v[i].Last() == '#';
				spread.Add(key, value);
			}

			return (init, spread);
		}

		private static int BoolArrToIn(bool[] bools)
		{
			var value = 0;
			for (int bit = 0; bit < bools.Length; bit++)
			{
				value += (int)((bools[bit] ? 1 : 0) * Math.Pow(2, bit));
			}
			return value;
		}

		private static int Part1((bool[], Dictionary<int, bool>) input, long nbGens, bool shrink)
		{

			var line = input.Item1;
			PrintLine(line);
			for (long i = 0; i < nbGens; i++)
			{
				line = AdvanceLine(line, input.Item2, shrink);
				PrintLine(line);
				//if (i > 10 && (i % 10000000) == 0)
				//	PrintLine(line);
			}
			NoelConsole.Write("");

			int score = 0;
			int firstIndex = input.Item1.FirstBy(x => x).Item2;
			for (int i = firstIndex; i < line.Length; i++)
			{
				if (line[i])
					score += i - firstIndex;
			}

			return score;
		}

		private static bool[] AdvanceLine(bool[] line, Dictionary<int, bool> spread, bool shrink)
		{
			var newLine = new bool[line.Length];
			for (int i = 2; i < line.Length - 2; i++)
			{
				var key = new bool[] { line[i - 2], line[i - 1], line[i], line[i + 1], line[i + 2] };
				var keyV = BoolArrToIn(key);
				if (spread.ContainsKey(keyV))
					newLine[i] = spread[keyV];
				else
					newLine[i] = false;
			}
			if (line[2])
				newLine = newLine.Prepend(false).Prepend(false).ToArray();
			if (line[3])
				newLine = newLine.Prepend(false).ToArray();


			if (line[line.Length - 3])
				newLine = newLine.Append(false).Append(false).ToArray();
			if (line[line.Length - 4])
				newLine = newLine.Append(false).ToArray();

			if (shrink)
			{
				var sline = newLine.Skip(0);
				while (!sline.Skip(3).First())
					sline = sline.Skip(1);
				newLine = sline.ToArray();
			}
			return newLine;
		}

		private static void PrintLine(bool[] arr)
		{
			string str = "";
			foreach (var c in arr)
				str += c ? '#' : '.';
			NoelConsole.Write(str);
		}

		private static int Part2((bool[], Dictionary<bool[], bool>) input)
		{

			return 1;

		}




	}
}