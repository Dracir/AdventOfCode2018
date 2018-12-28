using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Parser
	{

		public static int[] ParseIntArray(string str, char separator)
		{
			return str.Split(separator).Select(x => Int32.Parse(x)).ToArray();
		}

		public static Point4D ParseLinePoint4D(string str, char seperator)
		{
            var vals = str.Split(seperator);

            return new Point4D(
                Int32.Parse(vals[0]),
                Int32.Parse(vals[1]),
                Int32.Parse(vals[2]),
                Int32.Parse(vals[3])
            );
		}
	}
}