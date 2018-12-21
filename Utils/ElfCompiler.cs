using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class ElfCompiler
	{
		public delegate int ElfCode(int argA, int argB, int[] registers);

		public struct ElfTruction
		{
			public int OpCode;
			public string OpCodeName;
			public ElfCode Code;

			public ElfTruction(int opCode, string opCodeName, ElfCode code)
			{
				this.OpCode = opCode;
				this.OpCodeName = opCodeName;
				this.Code = code;
			}
		}

		public static ElfTruction addr = new ElfTruction(-1, "addr", (argA, argB, registers) => registers[argA] + registers[argB]);
		public static ElfTruction addi = new ElfTruction(-1, "addi", (argA, argB, registers) => registers[argA] + argB);

		public static ElfTruction mulr = new ElfTruction(-1, "mulr", (argA, argB, registers) => registers[argA] * registers[argB]);
		public static ElfTruction muli = new ElfTruction(-1, "muli", (argA, argB, registers) => registers[argA] * argB);

		public static ElfTruction banr = new ElfTruction(-1, "banr", (argA, argB, registers) => registers[argA] & registers[argB]);
		public static ElfTruction bani = new ElfTruction(-1, "bani", (argA, argB, registers) => registers[argA] & argB);

		public static ElfTruction borr = new ElfTruction(-1, "borr", (argA, argB, registers) => registers[argA] | registers[argB]);
		public static ElfTruction bori = new ElfTruction(-1, "bori", (argA, argB, registers) => registers[argA] | argB);

		public static ElfTruction setr = new ElfTruction(-1, "setr", (argA, argB, registers) => (registers[argA]));
		public static ElfTruction seti = new ElfTruction(-1, "seti", (argA, argB, registers) => (argA));


		public static ElfTruction gtir = new ElfTruction(-1, "gtir", (argA, argB, registers) => (argA > registers[argB] ? 1 : 0));
		public static ElfTruction gtri = new ElfTruction(-1, "gtri", (argA, argB, registers) => (registers[argA] > argB ? 1 : 0));
		public static ElfTruction gtrr = new ElfTruction(-1, "gtrr", (argA, argB, registers) => (registers[argA] > registers[argB] ? 1 : 0));


		public static ElfTruction eqir = new ElfTruction(-1, "eqir", (argA, argB, registers) => (argA == registers[argB] ? 1 : 0));
		public static ElfTruction eqri = new ElfTruction(-1, "eqri", (argA, argB, registers) => (registers[argA] == argB ? 1 : 0));
		public static ElfTruction eqrr = new ElfTruction(-1, "eqrr", (argA, argB, registers) => (registers[argA] == registers[argB] ? 1 : 0));

		public static ElfTruction[] opCodes = new ElfTruction[] { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr };


		public static int[] Compile(ElfTruction elfTruction, int argA, int argB, int argC, int[] registers)
		{
			var newRegisters = (int[])registers.Clone();
			newRegisters[argC] = elfTruction.Code(argA, argB, registers);
			return newRegisters;
		}

		public static int[] Compile(string opcode, int argA, int argB, int argC, int[] registers)
		{
			return Compile(GetFunc(opcode), argA, argB, argC, registers);
		}

		public static ElfTruction GetFunc(string opcode){
			return opCodes.First(x => x.OpCodeName == opcode);
		}
	}
}