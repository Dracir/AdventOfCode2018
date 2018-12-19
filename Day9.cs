using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day9
	{
		public static void Work()
		{

			//string test = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
			//string[] input = File.ReadAllLines("Data/D9Input.txt");

			NoelConsole.Write(Part1(48, 9, true) + "");

			Asset.AreEqual(8317, Part1(1618, 10), "Part1 Test 1618-10");
			Asset.AreEqual(146373, Part1(7999, 13), "Part1 Test 7999-13");
			Asset.AreEqual(2764, Part1(1104, 17), "Part1 Test 1104-17");
			Asset.AreEqual(54718, Part1(6111, 21), "Part1 Test 6111-21");
			Asset.AreEqual(37305, Part1(5807, 30), "Part1 Test 5807-30");

			//NoelConsole.WriteWithTime(() => "" + Part1(input));
			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}


		private static int Part1(int lastMarble, int nbPlayers, bool printSTuff = false)
		{
			int currentMarble = 0;
			int currentPlayer = 0;
			var marbles = new List<int>();
			marbles.Add(0);
			if (printSTuff)
				Print(-1, currentMarble, marbles);
			var scores = new int[nbPlayers];

			for (int i = 1; i <= lastMarble; i++)
			{
				currentPlayer = (i+1) % nbPlayers;
				if (i % 23 == 0)
				{
					scores[currentPlayer] += i;
					var backmove = Move(currentMarble - 6, marbles.Count());
					scores[currentPlayer] += marbles[backmove];
					marbles.RemoveAt(backmove);
					currentMarble = backmove - 1;
				}
				else
				{
					currentMarble = (currentMarble + 2) % (marbles.Count());
					marbles.Insert(currentMarble + 1, i);
				}
				if (printSTuff)
					Print(currentPlayer, currentMarble + 1, marbles);
			}
			NoelConsole.Write("\nResults : " + String.Join(",", scores));
			return scores.Max();
		}

		private static int Move(int index, int count)
		{
			if (index < 0)
				return count + index - 1;
			else
				return index % count;
		}

		private static void Print(int currentPlayer, int currentMarble, List<int> marbles)
		{
			var str = $"[{currentPlayer + 1}] ";
			for (int i = 0; i < marbles.Count; i++)
			{
				if (i == currentMarble)
					str += String.Format("({0:000})", marbles[i]);
				else
					str += String.Format(" {0:000} ", marbles[i]);
			}
			NoelConsole.Write(str);
		}

		private static int Part2(string[] input)
		{

			return 1;

		}
	}
}