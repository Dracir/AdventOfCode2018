using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day11
	{
		public static void Work()
		{
			int input = 7511;
			//string test = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
			//string[] input = File.ReadAllLines("Data/D9Input.txt");

			Asset.AreEqual(4, CalculatePower(Tuple.Create(3, 5), 8), "Part1 Test 1");
			Asset.AreEqual(-5, CalculatePower(Tuple.Create(122, 79), 57), "Part1 Test 1");
			Asset.AreEqual(0, CalculatePower(Tuple.Create(217, 196), 39), "Part1 Test 1");
			Asset.AreEqual(4, CalculatePower(Tuple.Create(101, 153), 71), "Part1 Test 1");

			Asset.AreEqual("33,45", Part1(18), "Part1 For 18");
			Asset.AreEqual("21,61", Part1(42), "Part1 For 42");
			NoelConsole.WriteWithTime(() => "" + Part1(input));

			//Part1Test(input);

			PrintGrid(MakeGrid(18,5));
			Asset.AreEqual("3,1,3", Part2(MakeGrid(18,5),5), "Part2 For 18 of size 5");


			/* Asset.AreEqual("90,269,16", Part2(MakeGrid(18,300),300), "Part2 For 18");
			Asset.AreEqual("232,251,12", Part2(MakeGrid(42,300),300), "Part2 For 42");
			NoelConsole.WriteWithTime(() => "" + Part2(MakeGrid(input,300),300)); */
		}

		private static void PrintGrid(int[,] values){
			var arrayStr = "";
			for (int i = 1; i < values.GetLength(0); i++)
			{
				for (int j = 1; j < values.GetLength(1); j++)
				{
					arrayStr += values[i, j].ToString(" #;-#; 0") + ",";
				}
				arrayStr += "\n";
			}
			NoelConsole.Write(arrayStr);
		}

		private static int[,] MakeGrid(int input, int size)
		{
			var powers = GridValues(size, size).Select(x => (x.Item1, x.Item2, CalculatePower(x, input)));
			int[,] powerCells = new int[size+1, size+1];
			powers.ForEach(x => powerCells[x.Item1, x.Item2] = x.Item3);
			return powerCells;
		}




		private static String Part1(int input)
		{
			var powers = GridValues(300, 300).Select(x => (x.Item1, x.Item2, CalculatePower(x, input)));
			int[,] powerCells = new int[301, 301];
			powers.ForEach(x => powerCells[x.Item1, x.Item2] = x.Item3);

			var max = GridValues(298, 298).MaxBy(x => CalculateArea(x.Item1, x.Item2, powerCells));
			return $"{max.Item1},{max.Item2}";
		}

		private static int CalculateArea(int x, int y, int[,] powerCells, int size = 3)
		{
			int area = 0;
			for (int i = 0; i < size; i++)
				for (int j = 0; j < size; j++)
					area += GetPower(x + i, y + j, powerCells);

			return area;
		}

		private static int GetPower(int x, int y, int[,] powers)
		{
			if (x > 300 || y > 300) return 0;
			return powers[x, y];

		}

		private static int CalculatePower(Tuple<int, int> pt, int input)
		{
			int x = pt.Item1;
			int y = pt.Item2;
			int rackID = x + 10;

			int power = rackID * ((rackID * y) + input);
			if (power < 100)
				power = 0;
			else
			{
				power = (power / 100) % 10;
			}

			power -= 5;
			return power;
		}

		static IEnumerable<Tuple<int, int>> GridValues(int width, int height)
		{
			for (int x = 1; x <= width; x++)
				for (int y = 1; y <= height; y++)
					yield return Tuple.Create(x, y);

		}

		static IEnumerable<Tuple<int, int, int>> GridValues(int width, int height, int size)
		{
			for (int s = 1; s <= size; s++)
				for (int x = 1; x <= width - (s - 1); x++)
					for (int y = 1; y <= height - (s - 1); y++)
						yield return Tuple.Create(x, y, size);
		}



		private static string Part2(int[,] powerCells, int size)
		{

			var max = GridValues(size, size)
			.Select(x =>
			{
				return (x.Item1, x.Item2, CalculateMaxArea(x.Item1, x.Item2, powerCells,size));
			})
			.MaxBy(x => x.Item3);
			return $"{max.Item1},{max.Item2},{max.Item3} of size {CalculateArea(max.Item1,max.Item2,powerCells,max.Item3)}";
		}



		private static int CalculateMaxArea(int x, int y, int[,] powerCells, int size)
		{
			int area = 0;
			int maxArea = 0;
			int maxAreaIndex = 0;
			int maxSize = size - Math.Max(x, y);
			for (int s = 1; s <= maxSize; s++)
			{
				for (int i = 0; i < s; i++)
				{
					area += powerCells[x + s - 1, y + i];
					area += powerCells[x + i, y + s - 1];
				}
				area += powerCells[x + s - 1, y + s - 1];

				if (area > maxArea)
				{
					maxArea = area;
					maxAreaIndex = s;
				}
			}


			return maxAreaIndex;
		}
	}
}
