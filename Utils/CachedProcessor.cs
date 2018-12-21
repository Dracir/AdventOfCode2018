using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2018
{
	public class CachedProcessor<T>
	{
		public List<T> CachedValues = new List<T>();
		public Dictionary<T, int> Lookup;
		public Dictionary<int, int> Transitions = new Dictionary<int, int>();

		Func<T, T> Simulate;
		public Action<T> PrintTAction;
		public bool Print = false;

		public CachedProcessor(IEqualityComparer<T> comparer, Func<T, T> simulate)
		{
			Lookup = new Dictionary<T, int>(comparer);
			this.Simulate = simulate;
		}

		public T Run(T startValue, long nbSimulation)
		{
			CachedValues.Add(startValue);
			Lookup.Add(startValue, 0);

			int StartingYpositionConsole = NoelConsole.WritingYPosition + 1;
			int OutputYPosition = NoelConsole.WritingYPosition + 1 + 6;

			int currentIndex = 0;
			int firstIndexLoop = 0;
			int loopbackIndex = 0;
			PrintStats(StartingYpositionConsole, OutputYPosition, firstIndexLoop, loopbackIndex, 0);


			for (long i = 1; i <= nbSimulation; i++)
			{
				if (currentIndex >= Transitions.Count)
				{
					var newValue = Simulate(CachedValues[currentIndex]);
					int nextValue = 0;
					if (Lookup.TryGetValue(newValue, out nextValue))
					{
						loopbackIndex = nextValue;
						firstIndexLoop = (int)i;
						currentIndex = loopbackIndex + (int)((nbSimulation - i) % (firstIndexLoop - loopbackIndex)) ;
						PrintStats(StartingYpositionConsole, OutputYPosition, firstIndexLoop, loopbackIndex, i);
						break;
					}
					else
					{
						CachedValues.Add(newValue);
						Lookup.Add(newValue, CachedValues.Count - 1);
						nextValue = CachedValues.Count - 1;
					}

					Transitions.Add(currentIndex, nextValue);
				}
				currentIndex = Transitions[currentIndex];

				if (Print && (i % 1000) == 0)
				{
					PrintStats(StartingYpositionConsole, OutputYPosition, firstIndexLoop, loopbackIndex, i);
				}
				if (Print && PrintTAction != null)
				{
					NoelConsole.WritingYPosition = OutputYPosition;
					PrintTAction?.Invoke(CachedValues[currentIndex]);
					OutputYPosition = NoelConsole.WritingYPosition;
					if (OutputYPosition > 70)
						OutputYPosition = StartingYpositionConsole + 6;
					Thread.Sleep(10);
				}
			}


			PrintStats(StartingYpositionConsole, OutputYPosition, firstIndexLoop, loopbackIndex, nbSimulation);

			return CachedValues[currentIndex];
		}

		private void PrintStats(int StartingYpositionConsole, int OutputYPosition, int firstIndexLoop, int loopbackIndex, long i)
		{
			NoelConsole.WritingYPosition = StartingYpositionConsole;
			NoelConsole.Write("i: " + i);
			NoelConsole.Write("Nb Diff Maps: " + CachedValues.Count);
			NoelConsole.Write("Nb Transition: " + Transitions.Count);
			NoelConsole.Write("Loop Back Index: " + loopbackIndex);
			NoelConsole.Write("Loop size: " + (firstIndexLoop - loopbackIndex));
			NoelConsole.Write("First Index start Loop: " + firstIndexLoop);
			NoelConsole.WritingYPosition = OutputYPosition;
		}
	}
}