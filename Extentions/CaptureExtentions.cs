using System;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public static class CaptureExtentions
	{
		public static int IntValue(this GroupCollection gc, int index)
		{
			return Int32.Parse(gc[index].Value);
		}

	}
}