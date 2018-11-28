using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
	public class Asset
	{
		public static void AreEqual(string expected, string actual, string message)
		{
			if (expected == actual)
				PrintSucess(message);
			else
				PrintFail(expected + "", actual + "", message);
		}
		public static void AreEqual(int expected, int actual, string message)
		{
			if (expected == actual)
				PrintSucess(message);
			else
				PrintFail(expected + "", actual + "", message);
		}

		public static void AreEqual(double expected, double actual, string message)
		{
			if (expected == actual)
				PrintSucess(message);
			else
				PrintFail(expected + "", actual + "", message);
		}


		public static void AreEqual(List<double> expected, List<double> actual, string message)
		{
			var firstNotSecond = expected.Except(actual).ToList();
			var secondNotFirst = actual.Except(expected).ToList();
			if (!firstNotSecond.Any() && !secondNotFirst.Any())
				PrintSucess(message);
			else
				PrintFail(String.Join(",",expected), String.Join(",",actual), message);
		}

		private static void PrintFail(string expected, string actual, string message)
		{
			NoelConsole.BackgroundColor = ConsoleColor.Red;
			NoelConsole.Write("[Failed] " + message);
			NoelConsole.Write(String.Format("   Expected {0} got {1}", expected, actual));
			Console.ResetColor();
		}


		private static void PrintSucess(string message)
		{
			NoelConsole.BackgroundColor = ConsoleColor.Green;
			NoelConsole.Write("[Sucess] " + message);
			Console.ResetColor();
		}
	}
}