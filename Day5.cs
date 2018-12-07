using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day5
	{
		public static void Work()
		{

			string test1 = File.ReadAllText("Data/D5Test.txt");
			string input = File.ReadAllText("Data/D5Input.txt");

			NoelConsole.Write("*Day 5 - Part 1*");

			Asset.AreEqual("dabCBAcaDA", Part1(test1), "Test1");


			NoelConsole.WriteWithTime(() => "" + Part1(input).Count());



			NoelConsole.Write("\n*Day 5 - Part 2*");
			NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static string Part1(string input)
		{
			/* var letters = IEnumrableExtentions.AllCapsLetters().Select(c => $"{c}{(char)(c + 32)}|{(char)(c + 32)}{c}").JoinStr('|');
			var regex = new Regex($"(.*)({letters})(.*)");
			while (true)
			{
				var cap = regex.Matches(input);
				if (cap.Count() == 0)
					return input;
				else
				{
					input = cap[0].Groups[1].Value + "" + cap[0].Groups[3].Value;
				}
			} */

			var letters = new LinkedList<char>(input);
			int count = input.Count();

			var interacted = true;
			while (interacted)
			{
				var node = letters.First;
				interacted = false;
				while (node?.Next != null)
				{
					if (areReactionable(node.Value, node.Next.Value))
					{
						interacted = true;
						letters.Remove(node.Next);
						letters.Remove(node);
						//NoelConsole.Write(letters.Aggregate("", (a, c) => a + c));
					}
					node = node.Next;
				}


			}

			return letters.Aggregate("", (a, c) => a + c);
		}

		private static bool areReactionable(char value1, char value2)
		{
			return value1 + 32 == value2 + 0 || value1 + 0 == value2 + 32;
		}

		private static int Part2(string input)
		{

			return IEnumrableExtentions.AllCapsLetters()
			.Select(c => Part1(removeLetter(input, c)).Count())
			.Min();
		}

		private static String removeLetter(string str, char uppChar) => str.Replace(uppChar + "", "").Replace((char)(uppChar + 32) + "", "");

	}
}