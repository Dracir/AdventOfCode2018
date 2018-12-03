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


			var t = new Stopwatch();
			t.Start();
			NoelConsole.Write("Output for input : " + Part1(File.ReadAllLines("Data/D3P1Input.txt")));

			t.Stop();
			NoelConsole.Write(String.Format("Time : {0:0.00}s",t.ElapsedMilliseconds/1000f));
			//NoelConsole.Write("\n*Day  - Part 2*");
			//NoelConsole.Write("Output for input : " + Part2("1122"));
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

		private static int Part1(string[] input)
		{

			var rects = input.Select(str => ParseStrToRect(str));

			var totalWidth = rects.Max(r => r.X + r.Width);
			var totalHeight = rects.Max(r => r.Y + r.Height);

			int[,] fabric = new int[totalWidth, totalHeight];
			var finalFabric = rects.Aggregate(fabric, (fab, cut)=> AddCut(fab,cut));


			//NoelConsole.Write(finalFabric);

			return finalFabric.Cast<int>().Where(value=>value>1).Count();
		}

		private static int[,] AddCut(int[,] fabric, Rect cut)
		{
			var newFabric = (int[,])fabric.Clone();

			for (int x = cut.X; x < cut.X + cut.Width; x++)
			{
				for (int y = cut.Y; y < cut.Y + cut.Height; y++)
				{
					newFabric[x, y] += 1;
				}
			}
			return newFabric;

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
			return input.Length;
		}
	}
}