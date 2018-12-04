using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day3
	{
		public static void Work()
		{
			NoelConsole.Write("*Day 3 - Part 1*");

			string[] test1 = File.ReadAllLines("Data/D3P1Test1.txt");

			Asset.AreEqual(4, Part1(test1), "Part1 Test 1");


			//NoelConsole.WriteWithTime(() => "" + Part1(File.ReadAllLines("Data/D3P1Input.txt")));

			NoelConsole.Write("\n*Day 3 - Part 2*");

			NoelConsole.WriteWithTime(() => "" + Part2(File.ReadAllLines("Data/D3P1Input.txt")));

		}



		struct Rect
		{
			public int Id;
			public int X;
			public int Y;
			public int Width;
			public int Height;

			public Rect(int id, int x, int y, int width, int height)
			{
				this.Id = id;
				this.X = x;
				this.Y = y;
				this.Width = width;
				this.Height = height;
			}
		}
		struct Data
		{
			public List<int> Intact;
			public int[,] Fabric;

			public Data(List<int> intact, int[,] fabric)
			{
				this.Intact = intact;
				this.Fabric = fabric;
			}
		}

		private static int Part1(string[] input)
		{

			var rects = input.Select(str => ParseStrToRect(str));

			var totalWidth = rects.Max(r => r.X + r.Width);
			var totalHeight = rects.Max(r => r.Y + r.Height);

			int[,] fabric = new int[totalWidth, totalHeight];

			var data = new Data(rects.Select(r => r.Id).ToList(), fabric);
			var finalFabric = rects.Aggregate(data, (d, cut) => AddCut(d, cut));

			return finalFabric.Fabric.Cast<int>().Where(value => value > 1).Count();
		}

		private static Data AddCut(Data data, Rect cut)
		{
			var newFabric = (int[,])data.Fabric.Clone();
			var newIntact = data.Intact.ToList();
			for (int x = cut.X; x < cut.X + cut.Width; x++)
			{
				for (int y = cut.Y; y < cut.Y + cut.Height; y++)
				{
					if (newFabric[x, y] > 0)
					{
						newIntact.Remove(newFabric[x, y]);
						newIntact.Remove(cut.Id);
						newFabric[x, y] = -1;
					}

					else if (newFabric[x, y] < 0)
					{
						newIntact.Remove(cut.Id);
					}
					else
						newFabric[x, y] = cut.Id;

				}
			}
			return new Data(newIntact, newFabric);

		}




		private static void Print(Rect rect)
		{
			NoelConsole.Write(String.Format("ID {0} at {1},{2} of size {3},{4}", rect.Id, rect.X, rect.Y, rect.Width, rect.Height));
		}

		private static Rect ParseStrToRect(string str)
		{
			Regex regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");

			var capture = regex.Matches(str)[0].Groups;

			return new Rect(Int32.Parse(capture[1].Value), Int32.Parse(capture[2].Value), Int32.Parse(capture[3].Value), Int32.Parse(capture[4].Value), Int32.Parse(capture[5].Value));
		}



		private static int Part2(string[] input)
		{

			var rects = input.Select(str => ParseStrToRect(str));

			var totalWidth = rects.Max(r => r.X + r.Width);
			var totalHeight = rects.Max(r => r.Y + r.Height);

			int[,] fabric = new int[totalWidth, totalHeight];

			var data = new Data(rects.Select(r => r.Id).ToList(), fabric);
			var finalFabric = rects.Aggregate(data, (d, cut) => AddCut(d, cut));

			return finalFabric.Intact.First();
		}
	}
}