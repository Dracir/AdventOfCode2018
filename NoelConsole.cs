using System;
using System.Diagnostics;

namespace AdventOfCode2018
{
	public class NoelConsole
	{
		private static int Width { get { return Console.WindowWidth; } }
		private static int Height { get { return Console.WindowHeight - 1; } }
		private static int Right { get { return Console.WindowWidth - 1; } }
		private static int Bottom { get { return Console.WindowHeight - 2; } }

		private static int WriteLeft { get { return 3; } }
		private static int WriteRight { get { return Width - 4; } }
		private static int WriteTop { get { return 4; } }
		private static int WriteHeight { get { return Height - WriteTop - 3; } }
		private static int WriteWidth { get { return Width - 7; } }

		public static ConsoleColor BackgroundColor { set { Console.BackgroundColor = value; } }
		public static ConsoleColor ForegroundColor { set { Console.ForegroundColor = value; } }

		private static ConsoleColor[] LightColors = new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.White };
		private static ConsoleColor[] GiftBg = new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow };
		private static Random RandomGenerator = new Random();
		private static Random GiftGenerator = new Random();

		private static int LastWidth;
		private static int LastHeight;

		public static int WritingYPosition = 0;

		private static Point Position
		{
			get { return new Point(Console.CursorLeft, Console.CursorTop); }
			set
			{
				Console.CursorLeft = Math.Clamp(value.X, 0, Right);
				Console.CursorTop = Math.Clamp(value.Y, 0, Bottom);
			}
		}
		public static void Redraw()
		{

			if (LastWidth != Width || LastHeight != Height)
			{
				Console.Clear();
				LastWidth = Width;
				LastHeight = Height;
			}
			GiftGenerator = new Random(10);

			DrawContour();
			DrawGifts();

			Console.CursorVisible = false;
			Console.CursorLeft = 1;
			Console.CursorTop = 0;
			Console.CursorTop = Bottom - 1;
		}

		private static void DrawGifts()
		{
			int x = 3;
			int y = Height - 2;
			for (int day = 0; day < Progression.Stars.Length; day++)
			{
				int stars = Progression.Stars[day];
				int giftSize = 0;
				if (stars == 2)
					giftSize = DrawBigGift(day, x, y);
				else if (stars == 1)
					giftSize = DrawSmallGift(day, x, y);
				else
					giftSize = DrawCoal(day, x, y);


				var r = GiftGenerator.NextDouble();

				x += giftSize + (int)(r * r * 4);
				if (x > Width - 2 - 7)
				{
					x = 3;
					y--;
				}


			}

		}

		private static int DrawCoal(int day, int x, int y)
		{
			BackgroundColor = ConsoleColor.Black;

			Draw(String.Format("a_{0}_", day), x, y);
			Console.ResetColor();

			return 2 + day.DigitCount();
		}

		private static int DrawSmallGift(int day, int x, int y)
		{
			BackgroundColor = GiftBg[GiftGenerator.Next(GiftBg.Length)];

			Draw(String.Format("A[ {0} ]", day), x, y);
			Console.ResetColor();

			return 4 + day.DigitCount();
		}

		private static int DrawBigGift(int day, int x, int y)
		{

			BackgroundColor = GiftBg[GiftGenerator.Next(GiftBg.Length)];

			if (GiftGenerator.NextDouble() < 0.5)
				Draw(String.Format("W[* {0} *]", day), x, y);
			else
				Draw(String.Format("W[# {0} #]", day), x, y);

			Console.ResetColor();

			return 6 + day.DigitCount();
		}

		private static void DrawContour()
		{
			var light = "G%G$G%\n W! \n r0";
			var boucle = "G*\\@/*\n  RX";
			int lightLength = 3;
			int boucleLenght = 5;
			var center = "G*";
			int x = 0;
			int drawingI = 0;

			bool odd = Width % 2 == 1;
			int nbHalf = Width / 2 - 2;
			//if (odd) nbHalf++;

			Draw(center, 0, 0);
			Draw(center, Width / 2 - 1, 0);
			if (odd)
				Draw(center, Width / 2, 0);
			Draw(center, Width - 2, 0);

			while (x < nbHalf)
			{
				string toDraw = "";
				int xWidth = 0;
				if (x + lightLength > nbHalf && x + boucleLenght > nbHalf)
				{
					toDraw = "G" + new string('*', nbHalf - x);
					xWidth = toDraw.Length - 1;
				}
				else if (drawingI++ % 2 == 1)
				{
					toDraw = light;
					xWidth = lightLength;
				}
				else
				{
					toDraw = boucle;
					xWidth = boucleLenght;
				}

				Draw(toDraw, 1 + x, 0);
				x += xWidth;
				Draw(toDraw, Width - 2 - x, 0);

			}


			for (int y = 1; y < Height - 1; y++)
			{
				Draw(center, 0, y);
				Draw(center, Right, y);
			}
			for (int i = 0; i < Width; i++)
			{
				Draw(center, i, Bottom);
			}
		}

		public static void WriteWithTime(Func<String> func){
			var t = new Stopwatch();
			t.Start();
			NoelConsole.Write("Output for input : " + func());
			t.Stop();
			NoelConsole.Write(String.Format("Time : {0:0.00}s",t.ElapsedMilliseconds/1000f));
		}

		public static void Write(int[,] values){
			var arrayStr = "";
			for (int i = 0; i < values.GetLength(0); i++)
			{
				for (int j = 0; j < values.GetLength(1); j++)
				{
					arrayStr += values[i,j] + ","; 
				}
				arrayStr+= "\n";
			}
			Write(arrayStr);
		}

		public static void Write(string value)
		{
			Write(value, 0, WritingYPosition);
			WritingYPosition += value.Split("\n").Length;

		}

		private static void Write(string value, int x, int y)
		{
			if (value.Contains("\n"))
			{
				foreach (var line in value.Split("\n"))
					y += WriteLine(line, x, y);

			}
			else
			{
				WriteLine(value, x, y);
			}
		}

		private static int WriteLine(string value, int x, int y)
		{
			int lines = 1;
			foreach (var c in value)
			{
				if (y >= WriteHeight)
					return lines;
				if (x >= WriteWidth)
				{
					lines++;
					y++;
					x = 0;
				}
				Position = new Point(WriteLeft + x, WriteTop + y);
				Console.Write(c);
				x++;
			}

			return lines;
		}


		private static void Draw(string value, int x, int y)
		{
			if (value.Contains("\n"))
			{
				foreach (var line in value.Split("\n"))
					Draw(line, x, y++);
				return;
			}

			foreach (var c in value)
			{
				if (c.Equals('G'))
					ForegroundColor = ConsoleColor.DarkGreen;
				else if (c.Equals('a'))
					ForegroundColor = ConsoleColor.DarkGray;
				else if (c.Equals('A'))
					ForegroundColor = ConsoleColor.Gray;
				else if (c.Equals('W'))
					ForegroundColor = ConsoleColor.White;
				else if (c.Equals('Y'))
					ForegroundColor = ConsoleColor.Yellow;
				else if (c.Equals('r'))
					ForegroundColor = LightColors[RandomGenerator.Next(LightColors.Length)];
				else if (c.Equals('R'))
					ForegroundColor = ConsoleColor.Red;
				else
				{
					Position = new Point(x, y);
					Console.Write(c);
					x++;
				}

			}

		}

		/* private static void Draw(string value)
		{
			Console.Write(value);
		}*/
	}
}