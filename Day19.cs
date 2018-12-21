using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day19
	{
		public static void Work()
		{

			//string test = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
			var input = Parse(File.ReadAllLines("Data/D19Input.txt"));


			//Asset.AreEqual(138, Part1(test), "Part1 Test 1");
			NoelConsole.WriteWithTime(() => "" + Part1(input));
			//NoelConsole.WriteWithTime(() => "" + Part2(input));
		}

		private static (int ip, (ElfCompiler.ElfTruction opcode, int argA, int argB, int argC)[] instructions) Parse(string[] input)
		{
			int ip = Int32.Parse(new String(input[0].Skip(4).ToArray()));
			var inst = input
			.Skip(1)
			.Select(x => parseLine(x))
			.ToList();

			return (ip, inst.ToArray());
		}

		private static (ElfCompiler.ElfTruction opcode, int argA, int argB, int argC) parseLine(string line)
		{
			var splited = line.Split(' ');
			return
			(
				ElfCompiler.GetFunc(splited[0]),
				Int32.Parse(splited[1]),
				Int32.Parse(splited[2]),
				Int32.Parse(splited[3])
			);
		}


		private static int Part1((int ip, (ElfCompiler.ElfTruction opcode, int argA, int argB, int argC)[] instructions) input)
		{
			int ip = input.ip;
			var registre = new int[] { 1, 0, 0, 0, 0, 0 };
			int nbSteps = 0;
			while (registre[ip] >= 0 && registre[ip] < input.instructions.Count())
			{
				nbSteps++;
				var instruction = input.instructions[registre[ip]];

				registre = ElfCompiler.Compile(instruction.opcode, instruction.argA, instruction.argB, instruction.argC, registre);

				//NoelConsole.Write(String.Join(',', registre));

				registre[ip]++;
			}
			NoelConsole.Write("NbSteps : " + nbSteps);
			return registre[0];
		}

		private static int Part2(string[] input)
		{

			return 1;

		}
	}
}