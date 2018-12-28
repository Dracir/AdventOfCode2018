using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day23
	{
		public static void Work()
		{

			var test = ParseInput(File.ReadAllLines("Data/D23Test1.txt"));
			var test2 = ParseInput(File.ReadAllLines("Data/D23Test2.txt"));
			var input = ParseInput(File.ReadAllLines("Data/D23input.txt"));


			//Asset.AreEqual(7, Part1(test), "Part1 Test 1");
			//NoelConsole.WriteWithTime(() => "" + Part1(input));


			Asset.AreEqual("12,12,12", Part2(test2), "Part2 Test 1");
			NoelConsole.WriteWithTime(() => Part2(input));
		}

		private static List<(int x, int y, int z, int r)> ParseInput(string[] value)
		{
			Regex regex = new Regex("pos=<(-?\\d+),(-?\\d+),(-?\\d+)>, r=(-?\\d+)");
			return value.Select(x =>
			{
				var match = regex.Match(x);
				return (match.Groups.IntValue(1), match.Groups.IntValue(2), match.Groups.IntValue(3), match.Groups.IntValue(4));
			}).ToList();
		}

		private static int Part1(List<(int x, int y, int z, int r)> input)
		{
			var monMax = input.Select(x => x)
			.MaxBy(pt => pt.r);
			NoelConsole.Write("Max at " + monMax.x + "," + monMax.y + "," + monMax.z + " with Radius of " + monMax.r);

			return input.Select(x => x).Count(pt =>
			{
				var dis = Distance(pt, monMax);
				return dis <= monMax.r;
			});
		}


		private static int Distance((int x, int y, int z, int r) p1, (int x, int y, int z, int r) p2)
		{
			return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + Math.Abs(p1.z - p2.z);
		}


		private static string Part2(List<(int x, int y, int z, int r)> input)
		{

			var best = MostNeighbor(input, 10000000);
			return "";

		}

		private static (int x, int y, int z, int r) MostNeighbor(List<(int x, int y, int z, int r)> input, int factor)
		{
			var pts = input.Select(pt => Factorise(pt, factor)).ToList();

			int minX = pts.Min(v => v.x);
			int minY = pts.Min(v => v.y);
			int minZ = pts.Min(v => v.z);
			int minR = pts.Min(v => v.r);

			int maxX = pts.Max(v => v.x);
			int maxY = pts.Max(v => v.y);
			int maxZ = pts.Max(v => v.z);
			int maxR = pts.Max(v => v.r);

			NoelConsole.Write($"Mins ({minX},{minY},{minZ}), Maxs ({maxX},{maxY},{maxZ}. Range values : [{minR},{maxR}]");
			long nbPos = ((long)(maxX - minX)) * ((long)(maxY - minY)) * ((long)(maxZ - minZ));
			NoelConsole.Write($"NbPosibilities : {nbPos}");

			var ptsAroundSomething = GetPoints(minX, minY, minZ, maxX, maxY, maxZ)
			.AsParallel()
			.Where(pt => pts.Any(nanobot => Distance(nanobot, pt) <= nanobot.r))
			.ToList();
			NoelConsole.Write($"Nb AroundSomething : {ptsAroundSomething.Count}");

			return (1, 1, 1, 1);
		}

		private static (int x, int y, int z, int r) Factorise((int x, int y, int z, int r) pt, int factor) => (pt.x / factor, pt.y / factor, pt.z / factor, pt.r / factor);

		private static IEnumerable<(int x, int y, int z, int r)> GetPoints(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					for (int z = minZ; z <= maxZ; z++)
					{
						yield return (x, y, z, 0);
					}
				}
			}
		}
	}
}