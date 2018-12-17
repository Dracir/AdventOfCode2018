using System;
using System.Threading;

namespace AdventOfCode2018
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WindowWidth = 150;
			Console.WindowHeight = 55;
			Console.WindowTop = 10;
			NoelConsole.Redraw();
			Day11.Work();

			while (true)
			{
				NoelConsole.Redraw();
				Thread.Sleep(200);
			}

		}
	}
}
