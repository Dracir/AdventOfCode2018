using System.Collections.Generic;

namespace AdventOfCode2018
{
	public static class DictionnaryExtentions
	{
		public static Dictionary<t, int> AddValue<t>(this Dictionary<t, int> dic, t key, int valueToAdd)
		{
			if (!dic.ContainsKey(key))
				dic.Add(key, valueToAdd);
			else
				dic[key] += valueToAdd;
			return dic;
		}
	}
}