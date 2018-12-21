using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day16
	{
		public static void Work()
		{

			string test = File.ReadAllText("Data/D16Test.txt");
			string input = File.ReadAllText("Data/D16Input.txt");
			string math = File.ReadAllText("Data/D16Math.txt");

			/* NbDupplicate(math,10);
			Asset.AreEqual(1, Part1(test), "Part1 Test 1");
			NoelConsole.WriteWithTime(() => "" + Part1(input));*/
			NoelConsole.WriteWithTime(() => "" + Part1(math)); 

			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static void NbDupplicate(string input, int nbToShow)
		{
			var dups = input.Replace("\r", "")
			.Split("\n\n\n", StringSplitOptions.None)[0]
			.Split("\n\n", StringSplitOptions.None)
			.Select(sampleInput => SampleInputToArrays(sampleInput))
			.Select(sample => GetNumberDuplicate(sample))
			.Take(nbToShow)
			.ToArray();
			NoelConsole.Write("Nb Dupplicate : " + String.Join(',', dups));
			//.Count(nbDupplicate => nbDupplicate >= 3);
		}


		private static int Part1(string input)
		{
			return input.Replace("\r", "")
			.Split("\n\n\n", StringSplitOptions.None)[0]
			.Split("\n\n", StringSplitOptions.None)
			.Select(sampleInput => SampleInputToArrays(sampleInput))
			.Select(sample => GetNumberDuplicate(sample))
			.Count(nbDupplicate => nbDupplicate >= 3);
		}

		private static int GetNumberDuplicate((int[] before, int[] instructions, int[] after) sample)
		{
			int nbDupplicate = 0;
			foreach (var opcode in ElfCompiler.opCodes)
			{
				var result = ElfCompiler.Compile(opcode, sample.instructions[1], sample.instructions[2], sample.instructions[3], sample.before);
				if (Enumerable.Range(0, 4).Select(i => result[i] == sample.after[i]).Count(x => x) == 4)
					nbDupplicate++;
			}
			return nbDupplicate;
		}

		private static Func<string, int[]> cleanedLineToArray = (line) => line.Split(' ').Select(c => Int32.Parse(c)).ToArray();

		private static (int[] before, int[] instructions, int[] after) SampleInputToArrays(string x)
		{
			var splited = x.Split('\n');
			Func<string, int[]> registerValuesParser = (line) => cleanedLineToArray(new String(line.Skip(9).SkipLast(1).Where(c => c != ',').ToArray()));

			var before = registerValuesParser(splited[0]);
			var instructions = cleanedLineToArray(splited[1]);
			var after = registerValuesParser(splited[2]);
			return (before, instructions, after);
		}


		private static int Part2(string input)
		{
			var samples = input.Replace("\r", "")
			.Split("\n\n\n", StringSplitOptions.None)[0]
			.Split("\n\n", StringSplitOptions.None)
			.Select(sampleInput => SampleInputToArrays(sampleInput))
			.ToList();

			var opCodes = new Dictionary<int, ElfCompiler.ElfTruction>();
			var unknownFunction = new List<ElfCompiler.ElfTruction>(ElfCompiler.opCodes);

			while (samples.Count() != 0)
			{
				NoelConsole.Write($"Remaining Samples count : {samples.Count}");

				var dupp = samples
				.Select(sample => (sample, GetMatchingFunctions(sample, unknownFunction)))
				.Where(x => x.Item2.Count() == 1).ToList();

				if (dupp.Count() > 0)
				{
					foreach (var item in dupp)
					{
						var foundCode = item.sample.instructions[0];
						var foundFunc = item.Item2[0];
						if (opCodes.ContainsKey(foundCode)) continue;

						samples.RemoveAll(x => x.instructions[0] == foundCode);

						unknownFunction.Remove(item.Item2[0]);
						opCodes.Add(foundCode, foundFunc);
						NoelConsole.Write($" -> Found function {foundFunc.OpCodeName} opcode is {foundCode}");
					}
				}
				else
				{
					NoelConsole.Write($" Found nothing ");
					break;
				}
			}


			var program = input.Replace("\r", "")
			.Split("\n\n\n", StringSplitOptions.None)[1]
			.Split("\n", StringSplitOptions.None)
			.Where(x => x.Count() > 0)
			.Select(x => cleanedLineToArray(x));

			return program
			.Aggregate(new int[] { 0, 0, 0, 0 }, (register, instruction) => ElfCompiler.Compile(opCodes[instruction[0]], instruction[1], instruction[2], instruction[3], register))
			.First();
		}




		private static ElfCompiler.ElfTruction[] GetMatchingFunctions((int[] before, int[] instructions, int[] after) sample, List<ElfCompiler.ElfTruction> lookUp)
		{
			var funcs = new List<ElfCompiler.ElfTruction>();
			foreach (var opcode in lookUp)
			{
				var result = ElfCompiler.Compile(opcode, sample.instructions[1], sample.instructions[2], sample.instructions[3], sample.before);
				if (Enumerable.Range(0, 4).Select(i => result[i] == sample.after[i]).Count(x => x) == 4)
					funcs.Add(opcode);
			}
			return funcs.ToArray();
		}



	}
}