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
			//Asset.AreEqual(1092, Part1(input), "Part1 Input Answer");
			//NoelConsole.WriteWithTime(() => "" + Part1(input));
			NoelConsole.WriteWithTime(() => "" + Part2(input));
			NoelConsole.WriteWithTime(() => "" + VraiPar2());
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
			var registre = new int[] { 0, 0, 0, 0, 0, 0 };
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

		private static int VraiPar2()
		{
			int A = 0;
			int B = 0;
			int C = 10551305;
			int E = 0;
			int F = 0;

			B = 1;
			/* while (B <= C)
			{
				E = 1;
				while (E <= C)
				{
					if (B * E == C)
						A += B;
					else
						E++;
				}
				B++;
			} */
			while (B <= C)
			{
				if (C % B == 0)
					A += B;
				
				B++;
			}

			return A;

		}

		private static int Part2((int ip, (ElfCompiler.ElfTruction opcode, int argA, int argB, int argC)[] instructions) input)
		{
			var fullCode =
			input.instructions
			.Select(ins => ("", ins))
			.Select(x =>
			{
				string varArgA = Char.ConvertFromUtf32('A' + x.ins.argA);
				string varArgB = Char.ConvertFromUtf32('A' + x.ins.argB);
				string varArgC = Char.ConvertFromUtf32('A' + x.ins.argC);

				var code = "";
				var operation = "";
				if (x.ins.opcode.OpCodeName == "setr")
					operation = $"{varArgA}";
				else if (x.ins.opcode.OpCodeName == "seti")
					operation = $"{x.ins.argA}";
				else if (x.ins.opcode.OpCodeName == "addr")
					operation = $"{varArgA} + {varArgB}";
				else if (x.ins.opcode.OpCodeName == "addi")
					operation = $"{varArgA} + {x.ins.argB}";
				else if (x.ins.opcode.OpCodeName == "mulr")
					operation = $"{varArgA} * {varArgB}";
				else if (x.ins.opcode.OpCodeName == "muli")
					operation = $"{varArgA} * {x.ins.argB}";
				else if (x.ins.opcode.OpCodeName == "gtir")
					operation = $"{x.ins.argA} > {varArgB}";
				else if (x.ins.opcode.OpCodeName == "gtri")
					operation = $"{varArgA} > {x.ins.argB}";
				else if (x.ins.opcode.OpCodeName == "gtrr")
					operation = $"{varArgA} > {varArgB}";
				else if (x.ins.opcode.OpCodeName == "eqrr")
					operation = $"{varArgA} == {varArgB}";

				if (x.ins.argC == input.ip)
					code = $"goto {operation}";
				else
					code = $"{varArgC} = {operation}";


				//public static ElfTruction gtir = new ElfTruction(-1, "gtir", (argA, argB, registers) => (argA > registers[argB] ? 1 : 0));
				//public static ElfTruction gtri = new ElfTruction(-1, "gtri", (argA, argB, registers) => (registers[argA] > argB ? 1 : 0));
				//public static ElfTruction gtrr = new ElfTruction(-1, "gtrr", (argA, argB, registers) => (registers[argA] > registers[argB] ? 1 : 0));
				return (code, x.ins);
			})
			.ToList();

			bool found = false;
			do
			{
				found = false;
				var conditions = fullCode
				.Select((line, i) => (line, i))
				.Where(x => x.line.ins.opcode.IsCondition);
				foreach (var item in conditions.ToArray())
				{
					if (fullCode.Skip(item.i + 1).First().code.Contains("goto") && fullCode.Skip(item.i + 2).First().code.Contains("goto"))
					{

						var gotoLine = fullCode.Skip(item.i + 2).First().ins.argA;
						found = true;
						fullCode.RemoveAt(item.i);
						fullCode.RemoveAt(item.i);
						fullCode.RemoveAt(item.i);

						var code = $"while {item.line.code.Substring(4)}";
						fullCode.Insert(gotoLine, (code, (ElfCompiler.whil, 1, 1, 1)));
						continue;
					}
				}

			} while (found);

			var output = fullCode.Select(x =>
			{
				var assembly = $"{x.ins.opcode.OpCodeName} {x.ins.argA} {x.ins.argB} {x.ins.argC}";
				return x.code + "\t\t\t\t\t\\\\" + assembly;
			});

			File.WriteAllLines(@"Output\D19Code.txt", output);


			return 0;
		}
	}
}