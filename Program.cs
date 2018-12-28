using System;
using System.Threading;

namespace AdventOfCode2018
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WindowWidth = 180;
			Console.WindowHeight = 40;
			Console.WindowTop = 4;
			NoelConsole.Redraw();
			Day23.Work();

			while (true)
			{
				NoelConsole.Redraw();
				Thread.Sleep(200);
			}

		}
	}
}
