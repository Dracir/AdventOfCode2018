using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
	public class Day8
	{
		public static void Work()
		{

			string test = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
			string input = File.ReadAllText("Data/D8Input.txt");
			var testvalues = Parser.ParseIntArray(test, ' ');
			var inputvalues = Parser.ParseIntArray(input, ' ');

			Asset.AreEqual(138, Part1(testvalues), "Part1 Test 1");
			NoelConsole.WriteWithTime(() => "" + Part1(inputvalues));
			//NoelConsole.WriteWithTime(() => "" + Part2(inputvalues));
		}



		private static int Part1(int[] input)
		{
			var root = ParseNode(input);

			//PrintNodes(root);

			return SumMeta(root);
		}

		private static void PrintNodes(Node root)
		{
			root.Print();
			foreach (var item in root.Childs)
				PrintNodes(item);
		}

		private static int SumMeta(Node node)
		{
			if (node == null) return 0;
			int sum = node.MetaEntries.Sum();
			sum += node.Childs.Sum(c => SumMeta(c));
			return sum;
		}


		private static Node ParseNode(int[] input)
		{
			int nbChilds = input[0];
			int nbMeta = input[1];

			//NoelConsole.Write(String.Join(",", input));

			if (nbChilds == 0)
			{
				var meta = input.Skip(2).SkipLast(input.Count() - 2 - nbMeta).ToArray();
				return new Node(new Node[0] { }, meta, 2 + nbMeta);
			}
			else
			{
				var childs = new Node[nbChilds];
				var newI = input.Skip(2).SkipLast(nbMeta);
				int childsSize = 0;
				Node newChild;
				for (int i = 0; i < nbChilds; i++)
				{
					var sendingValues = newI.ToArray();
					newChild = ParseNode(newI.ToArray());
					newI = newI.Skip(newChild.Size);
					childs[i] = newChild;
					childsSize += newChild.Size;
				}

				var meta = input.Skip(2 + childsSize).ToArray();
				return new Node(childs, meta, 2+childsSize+nbMeta);
			}
		}

		/* private static Tuple<int,Node> ParseNode(int[] input)
		{
			int nbChilds = input[0];
			int nbMeta = input[1];

			var meta = input.Skip(input.Count() - nbMeta).ToArray();
			if (nbChilds == 0)
				return Tuple.Create(input.Count(), new Node(new Node[0] { }, meta));
			else
			{
				var childs = new Node[nbChilds];
				for (int i = 0; i < nbChilds; i++)
				{

				}
				return new Node(childs, meta);
			}
		} */

		class Node
		{
			public Node[] Childs;
			public int[] MetaEntries;
			public int Size;

			public Node(Node[] childs, int[] metaEntries, int size)
			{
				this.Childs = childs;
				this.MetaEntries = metaEntries;
				this.Size = size;
			}

			public void Print()
			{
				NoelConsole.Write(String.Format("Node of size {2} with {0} Childs and Meta of {1}", Childs.Count(), String.Join(",", MetaEntries), Size));
			}
		}

		private static int Part2(int[] input)
		{

			return 1;

		}
	}
}