using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Point4D
	{
		public int X;
		public int Y;
		public int Z;
		public int W;

		public Point4D(int x, int y, int z, int w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public int DistanceManhattan(Point4D p2)
		{
			return Math.Abs(X - p2.X) + Math.Abs(Y - p2.Y) + Math.Abs(Z - p2.Z) + Math.Abs(W - p2.W);
		}


		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (Point4D)obj;
			return other.X == X && other.Y == Y && other.Z == Z && other.W == W;
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString(){
			return $"({X},{Y},{Z},{W})";
		}
	}
}