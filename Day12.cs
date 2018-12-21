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
			//NoelConsole.WriteWithTime(() => "" + Part1(inpoutMath,20,false));
			//NoelConsole.WriteWithTime(() => "" + Part1(inputParsed, 50000000000, true));

			//Asset.AreEqual(138, Part1(test), "Part1 Test 1");
			//NoelConsole.WriteWithTime(() => "" + Part1(input));
			NoelConsole.WriteWithTime(() => "" + Part2(inpoutMath, 50000000000, true));
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
			return CalculateScore(input, (0, line));
		}

		private static int CalculateScore((bool[], Dictionary<int, bool>) input, (int skiped, bool[] values) line)
		{
			int score = 0;
			int firstIndex = input.Item1.FirstBy(x => x).Item2;
			for (int i = firstIndex; i < line.values.Length; i++)
			{
				if (line.values[i])
					score += i - firstIndex + line.skiped;
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


		private static (int skiped, bool[] values) AdvanceLine((int skiped, bool[] values) line, Dictionary<int, bool> spread, bool shrink)
		{
			var newLine = new bool[line.values.Length];
			int skiped = line.skiped;

			for (int i = 2; i < line.values.Length - 2; i++)
			{
				var key = new bool[] { line.values[i - 2], line.values[i - 1], line.values[i], line.values[i + 1], line.values[i + 2] };
				var keyV = BoolArrToIn(key);
				if (spread.ContainsKey(keyV))
					newLine[i] = spread[keyV];
				else
					newLine[i] = false;
			}
			if (line.values[2])
				newLine = newLine.Prepend(false).Prepend(false).ToArray();
			if (line.values[3])
				newLine = newLine.Prepend(false).ToArray();


			if (line.values[line.values.Length - 3])
				newLine = newLine.Append(false).Append(false).ToArray();
			if (line.values[line.values.Length - 4])
				newLine = newLine.Append(false).ToArray();


			if (shrink)
			{
				var sline = newLine.Skip(0);
				while (!sline.Skip(3).First())
				{
					sline = sline.Skip(1);
					skiped++;
				}

				newLine = sline.ToArray();
			}
			return (skiped, newLine);
		}

		private static void PrintLine(bool[] arr)
		{
			string str = "";
			foreach (var c in arr)
				str += c ? '#' : '.';
			NoelConsole.Write(str);
		}

		private static void PrintLine((int skiped, bool[] values) line)
		{
			string str = "";
			foreach (var c in line.values)
				str += c ? '#' : '.';

			str += "    Skiped " + line.skiped;
			NoelConsole.Write(str);
		}

		private static int Part2((bool[], Dictionary<int, bool>) input, long nbGens, bool shrink)
		{
			var processor = new CachedProcessor<(int skiped, bool[] values)>(new LineEqualityComparer(), (inVal) => AdvanceLine(inVal, input.Item2, shrink));
			processor.Print = true;
			//processor.PrintTAction = PrintLine;
			var result = processor.Run((0, input.Item1), nbGens);
			NoelConsole.Write("skip " + result.skiped);
			for (int i = 10; i >= 1; i--)
			{
				PrintLine(processor.CachedValues[processor.CachedValues.Count() - i]);
			}
			return CalculateScore(input, result);

		}

		private class LineEqualityComparer : IEqualityComparer<(int skiped, bool[] values)>
		{
			public bool Equals((int skiped, bool[] values) map1, (int skiped, bool[] values) map2)
			{
				if (map1.values.Length != map2.values.Length) return false;
				for (int i = 0; i < map1.values.Length; i++)
				{
					if (map1.values[i] != map2.values[i])
						return false;
				}
				return true;
			}
			public int GetHashCode((int skiped, bool[] values) bx)
			{
				return bx.values.GetHashCode();
			}
		}




	}
}