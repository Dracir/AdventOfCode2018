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
        
		public static int[] ParseIntArray(string str, char separator){
            return str.Split(separator).Select(x=>Int32.Parse(x)).ToArray();
        }
    }
}