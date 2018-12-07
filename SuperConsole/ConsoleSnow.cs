using System;
using System.Collections.Generic;

namespace AdventOfCode2018
{
	public static class ConsoleSnow
	{
		public static List<SnowParticule> Particules = new List<SnowParticule>();
		public static List<SnowParticule> ParticulesToRemove = new List<SnowParticule>();


		private static Random RandGenerator = new Random();
		public static void Simulate()
		{

			Particules.ForEach(p =>
			{
				p.LastLastX = p.LastX;
				p.LastLastY = p.LastY;
				p.LastX = p.X;
				p.LastY = p.Y;
				p.Y += p.Speed;
				if (p.Y > 200)
					ParticulesToRemove.Add(p);


			});

			ParticulesToRemove.ForEach(pr => Particules.Remove(pr));
			ParticulesToRemove.Clear();

			Particules.Add(new SnowParticule(RandGenerator.Next(0, 250), 0, 1));
		}



	}

	public class SnowParticule
	{
		public int LastX = -1;
		public int LastY = -1;
		public int LastLastX = -1;
		public int LastLastY = -1;
		public int X;
		public int Y;
		public int Speed;

		public SnowParticule(int x, int y, int speed)
		{
			this.X = x;
			this.Y = y;
			this.Speed = speed;
		}

	}
}