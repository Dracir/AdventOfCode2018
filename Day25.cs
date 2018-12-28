using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day25
	{
		public static void Work()
		{
			var test1 = Parse(File.ReadAllLines("Data/D25Test1.txt"));
			Asset.AreEqual(2, Part1(test1), "Part1 Test 1");

			var test2 = Parse(File.ReadAllLines("Data/D25Test2.txt"));
			Asset.AreEqual(4, Part1(test2), "Part1 Test 2");

			var test3 = Parse(File.ReadAllLines("Data/D25Test3.txt"));
			Asset.AreEqual(3, Part1(test3), "Part1 Test 3");

			var test4 = Parse(File.ReadAllLines("Data/D25Test4.txt"));
			Asset.AreEqual(8, Part1(test4), "Part1 Test 4");
			
			var input = Parse(File.ReadAllLines("Data/D25Input.txt"));
			NoelConsole.WriteWithTime(() => "" + Part1(input));
			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static List<Point4D> Parse(string[] input)
		{
			return input.Select(x => Parser.ParseLinePoint4D(x, ',')).ToList();
		}

		private static int Part1(List<Point4D> input)
		{
			var constelations = new List<List<Point4D>>();

			foreach (var pt in input)
			{
				var distances = constelations
				.Select((pts, i) => (i, pts))
				.Select(x => (x.i, x.pts.Where(y => y.DistanceManhattan(pt) <= 3 && y.DistanceManhattan(pt) != 0)))
				.Where(x => x.Item2.Count() > 0);

				//NoelConsole.Write(distances.Count());

				if (distances.Count() > 1)
				{
					var nears = distances
					.Select(x => constelations[x.i]).ToList();

				//	NoelConsole.Write("New merges");
					var newConstelation = new List<Point4D>();
					newConstelation.Add(pt);
					foreach (var item in nears)
					{
						constelations.Remove(item);
						newConstelation.AddRange(item);
					}
					constelations.Add(newConstelation);
				}
				else if (distances.Count() == 0)
				{
				//	NoelConsole.Write("New");
					var newConstelation = new List<Point4D>();
					newConstelation.Add(pt);
					constelations.Add(newConstelation);
				}
				else
				{
					//var others = distances.First().Item2.Select(x=>x.ToString()).JoinStr('-');
					//NoelConsole.Write("pt " + pt.ToString() + " is near " + others);

					constelations[distances.First().i].Add(pt);
				}
			}

			return constelations.Count;
		}



		private static int Part2(string[] input)
		{

			return 1;

		}
	}
}