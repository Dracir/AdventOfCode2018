using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Point
	{
		public int X;
		public int Y;

		public Point Up { get { return new Point(X, Y + 1); } }
		public Point Down { get { return new Point(X, Y - 1); } }
		public Point Left { get { return new Point(X - 1, Y); } }
		public Point Right { get { return new Point(X + 1, Y); } }

		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}