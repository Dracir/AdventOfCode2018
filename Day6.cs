using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day6
	{
		public static void Work()
		{
			NoelConsole.Write("*Day 6 - Part 1*");

			string[] test = File.ReadAllLines("Data/D6Test.txt");
			string[] input = File.ReadAllLines("Data/D6Input.txt");
			string[] inputF = File.ReadAllLines("Data/D6InputFelix.txt");

			var testPts = ParsePoints(test);
			var inputPts = ParsePoints(input);
			var inputsF = ParsePoints(inputF);

			//Asset.AreEqual(17, Part1(testPts), "Part1 Test 1");


			//NoelConsole.WriteWithTime(() => "" + Part1(inputPts));
			NoelConsole.WriteWithTime(() => "" + Part1(inputsF));

			//NoelConsole.Write("\n*Day 3 - Part 2*");

			//NoelConsole.WriteWithTime(() => "" + Part2(input));

		}

		private static List<Point> ParsePoints(string[] test)
		{
			return test.Select(str =>
			{
				var splitted = str.Split(",");
				return new Point(Int32.Parse(splitted[0]), Int32.Parse(splitted[1]));
			})
			.ToList();
		}

		private static int Part1(List<Point> input)
		{
			int minX = input.Min(p => p.X);
			int maxX = input.Max(p => p.X);
			int minY = input.Min(p => p.Y);
			int maxY = input.Max(p => p.Y);

			int width = maxX - minX + 1;
			int height = maxY - minY + 1;

			var grid = new int[width, height];
			AllPoints(grid).ForEach(p => grid[p.X, p.Y] = -1);

			var points = input.Select(p => new Point(p.X - minX, p.Y - minY))
			.Select((p, i) => new { i, p });

			points.ForEach(item =>
			{
				grid[item.p.X, item.p.Y] = item.i;
			});

			AllPoints(grid).Where(p => grid[p.X, p.Y] == -1).ForEach(p1 =>
			{
				int minDist = Int32.MaxValue;
				points.ForEach(
					p2 =>
					{
						var dist = Distance(p1, p2.p);
						if (dist < minDist)
						{
							minDist = dist;
							grid[p1.X, p1.Y] = p2.i;
						}
						else if (dist == minDist)
						{
							grid[p1.X, p1.Y] = -2;
						}
					}
				);

			});

			var pointsSize = new int[points.Count()];
			AllPoints(grid).Where(p => grid[p.X, p.Y] >= 0).ForEach(p => pointsSize[grid[p.X, p.Y]]++);

			Print(grid, input.Select(p => new Point(p.X - minX, p.Y - minY)).ToList());
			return points.Where(p => !IsOnBorder(grid, p.p)).Max(p => pointsSize[p.i]);
		}

		private static int Distance(Point p1, Point p2) => Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);


		private static IEnumerable<Point> AllPoints(int[,] grid)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
				for (int y = 0; y < grid.GetLength(1); y++)
					yield return new Point(x, y);
		}

		private static int[,] Fill(int[,] grid, Point point, int pointIndex)
		{
			if (!IsValidPt(grid, point)) return grid;
			if (NbNeighboor(grid, point) > 1)
				grid[point.X, point.Y] = -2;
			else
				grid[point.X, point.Y] = pointIndex;
			return grid;
		}

		private static bool IsValidPt(int[,] grid, Point point) => point.X >= 0 && point.Y >= 0 && point.X < grid.GetLength(0) && point.Y < grid.GetLength(1);


		public static Point Left(Point p) => new Point(p.X - 1, p.Y);
		public static Point Right(Point p) => new Point(p.X + 1, p.Y);
		public static Point Up(Point p) => new Point(p.X, p.Y - 1);
		public static Point Down(Point p) => new Point(p.X, p.Y + 1);

		private static bool HasNeighboor(int[,] grid, Point pt)
		{
			return !(
				(pt.X <= 0 || grid[pt.X - 1, pt.Y] == -1) &&
				(pt.Y <= 0 || grid[pt.X, pt.Y - 1] == -1) &&

				(pt.X >= grid.GetLength(0) - 1 || grid[pt.X + 1, pt.Y] == -1) &&
				(pt.Y >= grid.GetLength(1) - 1 || grid[pt.X, pt.Y + 1] == -1)
			);

		}

		private static bool IsOnBorder(int[,] grid, Point pt) => pt.X == 0 || pt.Y == 0 || pt.X == grid.GetLength(0) - 1 || pt.Y == grid.GetLength(1) - 1;

		private static int NbNeighboor(int[,] grid, Point pt)
		{
			int nb = 0;

			if (!(pt.X <= 0 || grid[pt.X - 1, pt.Y] == -1))
				nb++;
			if (!(pt.Y <= 0 || grid[pt.X, pt.Y - 1] == -1))
				nb++;

			if (!(pt.X >= grid.GetLength(0) - 1 || grid[pt.X + 1, pt.Y] == -1))
				nb++;
			if (!(pt.Y >= grid.GetLength(1) - 1 || grid[pt.X, pt.Y + 1] == -1))
				nb++;

			return nb;

		}

		static void Print(int[,] grid, List<Point> pts)
		{
			var str = "";
			Enumerable.Range(0, grid.GetLength(1)).ForEach(i =>
			  {
				  Enumerable.Range(0, grid.GetLength(0)).ForEach(j =>
				  {
					  var pointsExist = pts.Where(p=>p.X == j && p.Y == i);

					  int v = grid[j, i];
					  if(pointsExist.Count()>0)
						  str += '#';
					  else if (v == -1)
						  str += '@';
					  else if (v == -2)
						  str += '.';
					  else
					  	str += (char)('A' + v);
				  });
				  str += "\n";
			  });
			//NoelConsole.Write(str);
			File.WriteAllText("Output/D6P1.txt", str);
		}


		private static int Part2(string[] input)
		{

			return 1;

		}
	}
}