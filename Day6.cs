using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AdventOfCode2018
{
	public class Day6
	{
		public static void Work()
		{

			string[] test1 = File.ReadAllLines("Data/D5Test1.txt");
			string[] input = File.ReadAllLines("Data/D5Input.txt");

			NoelConsole.Write("*Day 6 - Part 1*");

			Asset.AreEqual(240, Part1(test1), "Test1");

			NoelConsole.WriteWithTime(() => "" + Part1(input));


			NoelConsole.Write("\n*Day 6 - Part 2*");
			NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static int Part1(string[] input)
		{
			return 1;
		}

		private static int Part2(string[] input)
		{
			return 1;
		}

	}
}