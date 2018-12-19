using System;
using System.Threading;

namespace AdventOfCode2018
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WindowWidth = 300;
			Console.WindowHeight = 80;
			Console.WindowTop = 4;
			NoelConsole.Redraw();
			Day18.Work();

			while (true)
			{
				NoelConsole.Redraw();
				Thread.Sleep(200);
			}

		}
	}
}
