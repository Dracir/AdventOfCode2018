using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day7
	{
		public static void Work()
		{
			NoelConsole.Write("*Day 7 - Part 1*");

			string[] test = File.ReadAllLines("Data/D7Test.txt");
			string[] input = File.ReadAllLines("Data/D7Input.txt");

			var steps = Parse(test);
			//steps.ForEach(t => NoelConsole.Write(t.Item1 + " before " + t.Item2));

			//Asset.AreEqual("CABDFE", Part1(steps), "Part1 Test 1");


			//NoelConsole.WriteWithTime(() => Part1(Parse(input)));

			NoelConsole.Write("\n*Day 7 - Part 2*");



			Asset.AreEqual(15, Part2(steps), "Part2 Test 1");

			//NoelConsole.WriteWithTime(() => "" + Part2(Parse(input)));

		}

		private static List<Tuple<char, char>> Parse(string[] test)
		{
			Regex regex = new Regex("Step (.*) must be finished before step (.*) can begin.");

			return test.Select(str => (regex.Match(str)).Groups)
			.Select(g => Tuple.Create(g[1].Value[0], g[2].Value[0]))
			.OrderBy(x => x.Item1)
			.ToList();
		}


		private static string Part1(List<Tuple<char, char>> steps)
		{
			var stepRemaining = steps.ToList();
			string instruction = "";
			while (stepRemaining.Count != 0)
			{
				var top = stepRemaining.First(x => !stepRemaining.Any(y => y.Item2 == x.Item1));
				instruction += top.Item1;
				stepRemaining.Remove(top);
				stepRemaining.RemoveAll(x => x.Item1 == top.Item1);
			}
			var missingStep = steps.First(x => !instruction.Contains(x.Item2));
			instruction += missingStep.Item2;
			return instruction;
		}

		class WorkerTask
		{
			public int WorkerId;
			public int StartTime;
			public int EndTime;
			public int Duration { get { return EndTime - StartTime; } }
			public Tuple<char, char> Step;

			public WorkerTask(int workerId, int startTime, int endTime, Tuple<char, char> step)
			{
				this.WorkerId = workerId;
				this.StartTime = startTime;
				this.EndTime = endTime;
				this.Step = step;
			}

			public override String ToString() => $"Worker {WorkerId} from {StartTime} to {EndTime} on {Step.Item2}.";

		}

		private static int Part2(List<Tuple<char, char>> steps)
		{
			int nbWorker = 2;
			var taskList = new List<WorkerTask>();
			var workerTask = new WorkerTask[nbWorker];

			var stepRemaining = steps.ToList();
			char lastStep = 'A';
			bool lastDone = false;
			string instruction = "";

			int currentTime = 0;
			while (stepRemaining.Count != 0 || workerTask.Any(x => x != null) || !lastDone)
			{
				var foundSomething = true;
				while (foundSomething)
				{
					foundSomething = false;

					if (stepRemaining.Count == 0)
					{
						if (!lastDone && !workerTask.Any(x => x != null && x.Step.Item2 == lastStep))
						{
							if (!workerTask.Any(x => x != null && x.Step.Item2 == lastStep))
							{
								lastDone = true;
								foundSomething = true;
								int firstIndexFree = -1;
								while (workerTask[++firstIndexFree] != null) ;
								var task = new WorkerTask(firstIndexFree, currentTime, currentTime + 60 * 0 + (lastStep - 'A') + 1, Tuple.Create(lastStep, lastStep));
								taskList.Add(task);
								workerTask[firstIndexFree] = task;
								instruction += lastStep;
							}
						}
					}
					else if (workerTask.Any(x => x == null))
					{
						var top = stepRemaining.First(x => !stepRemaining.Any(y => y.Item2 == x.Item1));
						if (!workerTask.Any(x => x != null && x.Step.Item2 == top.Item1))
						{
							foundSomething = true;
							int firstIndexFree = -1;
							while (workerTask[++firstIndexFree] != null) ;
							var task = new WorkerTask(firstIndexFree, currentTime, currentTime + 60 * 0 + (top.Item1 - 'A') + 1, top);
							taskList.Add(task);
							stepRemaining = stepRemaining.OrderBy(x => x.Item1).ToList();
							workerTask[firstIndexFree] = task;
							instruction += top.Item1;
							stepRemaining.RemoveAll(x => x.Item1 == top.Item1);
							if (stepRemaining.Count == 0)
							{
								lastStep = steps.First(x => !instruction.Contains(x.Item2)).Item2;
								lastDone = false;
							}
						}
					}
				}


				currentTime++;
				for (int i = 0; i < nbWorker; i++)
					if (workerTask[i]?.EndTime == currentTime)
						workerTask[i] = null;

			}

			//taskList.ForEach(t => NoelConsole.Write(t.ToString()));
			var maxTime = taskList.Max(x => x.EndTime);
			var timesheet = new char[nbWorker, maxTime];
			for (int y = 0; y < maxTime; y++)
				for (int x = 0; x < nbWorker; x++)
					timesheet[x, y] = ' ';

			foreach (var item in taskList)
				for (int y = item.StartTime; y < item.EndTime; y++)
					timesheet[item.WorkerId, y] = item.Step.Item1;

			string output = "";
			for (int y = 0; y < maxTime; y++)
			{
				output += y + "\t";
				for (int x = 0; x < nbWorker; x++)
				{
					output += timesheet[x, y] + "\t";
				}
				output += "\n";
			}
			File.WriteAllText("Output/D7output.txt", output);

			return currentTime+1;

		}
	}
}