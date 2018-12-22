using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Grid<T>
	{
		public T[,] Values;

		public T this[Point key]
		{
			get { return this[key.X, key.Y]; }
			set { this[key.X, key.Y] = value; }
		}

		public T this[int x, int y]
		{
			get { return Values[x, y]; }
			set
			{
				Values[x, y] = value;
				minX = Math.Min(minX, x);
				minY = Math.Min(minY, y);
				maxX = Math.Max(maxX, x);
				maxY = Math.Max(maxY, y);
			}
		}

		public IEnumerable<int> ColumnIndexs()
		{
			for (int y = minY; y <= maxY; y++)
				yield return y;
		}

		public IEnumerable<int> RowIndexs()
		{
			for (int x = minX; x <= maxX; x++)
				yield return x;
		}

		private int OffsetX = 1000;
		private int OffsetY = 1000;
		private int minX;
		private int minY;
		private int maxX;
		private int maxY;

		public int MinX { get { return minX; } }
		public int MinY { get { return minY; } }
		public int MaxX { get { return maxX; } }
		public int MaxY { get { return maxY; } }
		public int Width { get { return maxX - minX + 1; } }
		public int Height { get { return maxY - minY + 1; } }

		public Point TopLeft { get { return new Point(minX, maxY); } }


		public T[,] ToArray()
		{
			var arr = new T[Width, Height];
			for (int x = minX; x <= maxX; x++)
				for (int y = minY; y <= maxY; y++)
					arr[x - minX, y - minY] = Values[x, y];
			return arr;
		}

		public Point TopRight { get { return new Point(maxX, maxY); } }
		public Point BottomLeft { get { return new Point(minX, minY); } }
		public Point BottomRight { get { return new Point(maxX, minY); } }

		private T DefaultValue;

		public Point Center { get { return new Point(OffsetX, OffsetY); } }

		public Grid(T defaultValue)
		{
			this.DefaultValue = defaultValue;
			Values = new T[2 * OffsetX, 2 * OffsetY];
			minX = OffsetX;
			minY = OffsetY;
			maxX = OffsetX;
			maxY = OffsetY;
		}

	}

}
