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

			Asset.AreEqual(240, Part1(ParseInputForPart1(test1)), "Test1");

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

			return guardEventList;
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
			var shifts = new List<int[]>();
			Enumerable.Range(0, maxId - 1).ForEach(i => shifts.Add(new int[60]));

			input.Where(i => !i.IsBeginsShift)
				.PairUp()
				.ForEach(pair =>
				{
					for (int i = pair.Item1.Date.Minute; i <= pair.Item2.Date.Minute; i++)
						shifts[pair.Item1.ID-1][i]++;
				});

			/* var maxSleeper = shifts.MaxBy((g,i)=>g.Sum());


			var shifts = new int[maxId, 60];

			var shiftsValues = input.Where(i => !i.IsBeginsShift)
				.PairUp()
				.Aggregate(shifts, (s, pair) =>
					{
						for (int i = pair.Item1.Date.Minute; i <= pair.Item2.Date.Minute; i++)
							s[pair.Item1.ID, i]++;
						return s;
					});

			input.Select(i => i.ID)
			.Distinct()
			.Max((i) => Enumerable.Range(pair.Item1.Date.Minute, pair.Item2.Date.Minute)
						.Sum(x => shifts[i, x]));

			var data = new List<Part1Data>();

			var mostSleeperId = input.Aggregate(data, (d, eve) => ParseEvent(d, eve))
				.MaxBy(e => e.SleepingTime)
				.ID;
 */
			int[] sleeps = new int[60];
			/* input.Where(i => i.ID == mostSleeperId)
			.Aggregate(sleeps,(acc,
			;*/


			return 1;
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
		public bool IsSleeping;
		public DateTime StartSleep;
		public int SleepingTime;
	}

	struct GuardEvent
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
}