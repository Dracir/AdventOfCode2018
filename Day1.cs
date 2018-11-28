using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2018
{
	public class Day1
	{
		public static void Work()
		{
			NoelConsole.Write("*Day  - Part 1*");

			Asset.AreEqual(2, Part1("1122"), "1122");

			NoelConsole.Write("Output for input : " + Part1("1122"));


			//NoelConsole.Write("\n*Day  - Part 2*");
			//NoelConsole.Write("Output for input : " + Part2("1122"));
		}

		private static int Part1(string input)
		{
			return input.Length;
		}


		private static int Part2(string input)
		{
			return input.Length;
		}
	}
}