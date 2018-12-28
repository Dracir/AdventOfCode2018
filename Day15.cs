using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day15
	{
		public static void Work()
		{
			string test1 = "^ENWWW$";
			var test1GridExpected = StringToCharCharArr("#####\n#.|.#\n#-###\n#.|.#\n#####");
			string test2 = "^ENWWW(NEEE|SSE(EE|N))$";
			string test3 = "^WNE$";

			NoelConsole.Write(InputToGrid(test1).ToArray());
			//Asset.AreEqual(test1GridExpected, InputToGrid(test1).ToArray(), "Part1 Test 1 GridTest");
			//Asset.AreEqual(3, Part1(test1), "Part1 Test 1 ");
			//Asset.AreEqual(10, Part1(test2), "Part1 Test 2 " + test2);
			//Asset.AreEqual(18, Part1(test3), "Part1 Test 3 " + test3);
			//string test = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
			//string[] input = File.ReadAllLines("Data/D9Input.txt");


			//Asset.AreEqual(138, Part1(test), "Part1 Test 1");
			//NoelConsole.WriteWithTime(() => "" + Part1(input));
			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static char[,] StringToCharCharArr(string v)
		{
			var splited = v.Split('\n');
			var output = new char[splited[0].Length, splited.Length];
			for (int y = 0; y < splited.Length; y++)
				for (int x = 0; x < splited[0].Length; x++)
					output[x, y] = splited[y][x];
			return output;
		}

		private static int Part1(string input)
		{
			var g = InputToGrid(input);
			return 1;
		}

		private static Grid<char> InputToGrid(string input)
		{
			Grid<char> grid = new Grid<char>('?');
			Point pt = grid.Center;
			grid[pt] = '.';
			var regex = new String(input.Skip(1).SkipLast(1).ToArray());
			Run(grid, pt, regex);
			FillSurroundingWalls(grid);

			return grid;
		}

		private static void FillSurroundingWalls(Grid<char> grid)
		{
			var xLeft = grid.MinX;
			var xRight = grid.MaxX;
			var yBottom = grid.MinY;
			var yTop = grid.MaxY;
			if (grid.ColumnIndexs().Any(y => grid[grid.MinX, y] == '.'))
				xLeft--;
			if (grid.ColumnIndexs().Any(y => grid[grid.MaxX, y] == '.'))
				xRight++;

			if (grid.RowIndexs().Any(x => grid[x, grid.MinY] == '.'))
				yBottom--;
			if (grid.RowIndexs().Any(x => grid[x, grid.MaxY] == '.'))
				yTop++;

			for (int x = xLeft; x <= xRight; x++)
			{
				grid[x, yBottom] = '#';
				grid[x, yTop] = '#';
			}
			for (int y = yBottom; y < yTop; y++)
			{
				grid[xLeft, y] = '#';
				grid[xRight, y] = '#';
			}
		}

		private static void Run(Grid<char> grid, Point pt, String regex)
		{
			int i = 0;
			var currentPt = pt;
			var groups = getInstructionTree(regex);

			foreach (var c in regex)
			{
				if (c == 'W')
					currentPt = Avance(grid, currentPt.Left, currentPt.Left.Left, false);
				else if (c == 'E')
					currentPt = Avance(grid, currentPt.Right, currentPt.Right.Right, false);
				else if (c == 'N')
					currentPt = Avance(grid, currentPt.Up, currentPt.Up.Up, true);
				else if (c == 'S')
					currentPt = Avance(grid, currentPt.Down, currentPt.Down.Down, true);
			}

		}

		private static MapInstructionNode getInstructionTree(string regex)
		{
			if (!regex.Contains('(') && !regex.Contains(')') && !regex.Contains('|'))
			{
				return new MapInstructionNode(regex);
			}
			else if (regex.Contains('('))
			{
				var str = "";
				int currentNbParenteses = 0;
				foreach (var c in regex)
				{
					if (c == 'N' || c == 'E' || c == 'W' || c == 'S')
						str += c;
					else
					{
						//	if(str.Length
					}
				}
			}
			return null;

		}

		private class MapInstructionNode
		{
			public String Instruction;
			public List<MapInstructionNode> Nodes = new List<MapInstructionNode>();

			public MapInstructionNode(string regex)
			{
				this.Instruction = regex;
			}
		}

		private static Point Avance(Grid<char> grid, Point door, Point room, bool vertical)
		{
			grid[room] = '.';
			grid[door] = vertical ? '-' : '|';
			if (vertical)
			{
				grid[door.Left] = '#';
				grid[door.Right] = '#';
			}
			else
			{
				grid[door.Up] = '#';
				grid[door.Down] = '#';
			}
			return room;
		}

		private static int Part2(string[] input)
		{

			return 1;

		}
	}
}