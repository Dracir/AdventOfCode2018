using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day4
	{
		public static void Work()
		{

			string[] test1 = File.ReadAllLines("Data/D4Test1.txt");
			string[] input = File.ReadAllLines("Data/D4Input.txt");

			NoelConsole.Write("*Day 4 - Part 1*");

			//Asset.AreEqual(240, Part1(ParseInputForPart1(test1)), "Test1");

			NoelConsole.WriteWithTime(() => "" + Part1(ParseInputForPart1(input)));

			/* 
						NoelConsole.Write("\n*Day 4 - Part 2*");
						NoelConsole.WriteWithTime(() => "" + Part2(shifts)); */
		}

		private static List<GuardEvent> ParseInputForPart1(string[] input)
		{
			var guardEvents = input.Select(str => ParseGuardEvent(str)).ToArray();


			int lastId = guardEvents.First().ID;
			var guardEventList = new List<GuardEvent>();
			var bob = guardEvents.ToArray();
			for (int i = 0; i < bob.Count(); i++)
			{
				var item = bob[i];
				if (item.IsBeginsShift)
					lastId = item.ID;
				else
					item.ID = lastId;
				guardEventList.Add(item);
			}

			return guardEventList.OrderBy(x => x.Date).ToList();
		}

		private static GuardEvent ParseGuardEvent(string str)
		{
			Regex regex = new Regex(@"\[(\d){4}-(\d\d)-(\d\d) (\d\d):(\d\d)] (.*)");

			var capture = regex.Matches(str)[0].Groups;
			DateTime date = new DateTime(capture.IntValue(1), capture.IntValue(2), capture.IntValue(3), capture.IntValue(4), capture.IntValue(5), 0);

			var guardEvent = new GuardEvent(date);

			if (capture[6].Value.Contains("falls asleep"))
				guardEvent.IsFallAsleep = true;
			else if (capture[6].Value.Contains("wakes up"))
				guardEvent.IsAwakeUp = true;
			else
			{
				var captureTxt = new Regex(@".*#(\d+).*").Matches(capture[6].Value)[0].Groups;
				guardEvent.IsBeginsShift = true;
				guardEvent.ID = captureTxt.IntValue(1);
			}

			return guardEvent;

		}

		private static int Part1(List<GuardEvent> input)
		{
			var maxId = input.Max(i => i.ID);
			var shifts = new Dictionary<int, int[]>();

			input.DistinctBy(x => x.ID)
			.ForEach(x => shifts
			.Add(x.ID, new int[60]));

			var sleepEvent = input.Where(i => !i.IsBeginsShift)
				.PairUp()
				.Select(pair => new SleepEvent(pair.Item1.ID, pair.Item1.Date, pair.Item2.Date.Minute - pair.Item1.Date.Minute));

			//sleepEvent.ForEach(se => NoelConsole.Write(se.ToString()));
			var dod = sleepEvent.ToArray();
			sleepEvent.ForEach(se =>
			{
				for (int i = se.Start.Minute; i < se.Start.Minute + se.Duration; i++)
					shifts[se.ID][i] += 1;
			});

			var mostSleeper = shifts
			.MaxBy(s => s.Value.Sum());

			var maxMin = mostSleeper.Value
			.Select((time, min) => new { time, min })
			.MaxBy(x => x.time);

			PrintShifts(shifts);
			return mostSleeper.Key * maxMin.min;
		}

		private static void PrintShifts(Dictionary<int, int[]> shifts)
		{
			var a = "";
			var b = "";
			for (int i = 0; i < 6; i++)
				for (int j = 0; j < 10; j++)
				{
					a += i.ToString() + "\t";
					b += j.ToString() + "\t";
				}

			var output = "ID\tMinute\n\t\t" + a + "\n\t\t" + b;

			foreach (var s in shifts)
			{
				output += $"\n{s.Key:0000}\t";
				foreach (var min in s.Value)
				{
					output += min.ToString() + "\t";
				}
			}


			File.WriteAllText("Output/D4.txt", output);
		}

		private static List<Part1Data> ParseEvent(List<Part1Data> list, GuardEvent eve)
		{
			var eventData = list.Where(a => a.ID == eve.ID);
			Part1Data data;
			if (eventData.Count() == 0)
			{
				data = new Part1Data();
				data.ID = eve.ID;
			}
			else
			{
				data = eventData.First();
				list.Remove(data);
			}

			if (eve.IsFallAsleep)
				data.StartSleep = eve.Date;
			else if (eve.IsAwakeUp)
				data.SleepingTime += eve.Date.Subtract(data.StartSleep).Minutes;

			list.Add(data);

			return list;


		}

		private static int Part2(GuardShift[] input)
		{
			return 1;
		}

	}

	struct Part1Data
	{
		public int ID;
		//public bool IsSleeping;
		public DateTime StartSleep;
		public int SleepingTime;
	}

	class GuardEvent
	{
		public DateTime Date;

		public bool IsAwakeUp;
		public bool IsFallAsleep;
		public bool IsBeginsShift;
		public int ID;

		public GuardEvent(DateTime date)
		{
			this.Date = date;
			this.IsAwakeUp = false;
			this.IsBeginsShift = false;
			this.IsFallAsleep = false;
			this.ID = 0;
		}

		public override string ToString()
		{
			return $"{ID} at {Date}";
		}
	}

	struct GuardShift
	{
		public DateTime Date;
		public List<DateTime> Events;

		public GuardShift(DateTime date, List<DateTime> events)
		{
			this.Date = date;
			this.Events = events;
		}
	}

	class SleepEvent
	{

		public int ID;
		public DateTime Start;
		public int Duration;

		public SleepEvent(int id, DateTime start, int duration)
		{
			this.ID = id;
			this.Start = start;
			this.Duration = duration;
		}

		public override string ToString()
		{
			return $"{ID} at {Start.Month}-{Start.Day} sleep {Duration}mins.";
		}
	}
}