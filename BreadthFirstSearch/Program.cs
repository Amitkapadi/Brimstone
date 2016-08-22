﻿// Example of breadth-first game state searching

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Brimstone;

namespace Test1
{
	public class BreadthFirstSearch
	{
		// Configure test parameters here
		public const int MaxMinions = 7;
		public const int NumBoomBots = 2;
		public const string FillMinion = "Bloodfen Raptor";
		public static bool BoomBotTest = false;
		public static bool ArcaneMissilesTest = true;

		static void Main(string[] args) {
			bool gotit = false;
			do {
				// Create initial game state
				Console.WriteLine("Initializing game state...");

				var game = new Game(HeroClass.Druid, HeroClass.Druid, PowerHistory: true);
				game.Start(1);

				for (int i = 0; i < MaxMinions - NumBoomBots; i++)
					game.CurrentPlayer.Give(FillMinion).Play();
				for (int i = 0; i < NumBoomBots; i++)
					game.CurrentPlayer.Give("Boom Bot").Play();
				game.BeginTurn();
				for (int i = 0; i < MaxMinions - NumBoomBots; i++)
					game.CurrentPlayer.Give(FillMinion).Play();
				for (int i = 0; i < NumBoomBots; i++)
					game.CurrentPlayer.Give("Boom Bot").Play();

				var ArcaneMissiles = game.CurrentPlayer.Give("Arcane Missiles");

				// Start timing
				var sw = new Stopwatch();
				sw.Start();

				Cards.FromName("Arcane Missiles").Behaviour.Battlecry = Actions.Damage(Actions.RandomOpponentCharacter, 1) * 4;
				Cards.FromName("Boom Bot").Behaviour.Deathrattle = Actions.Damage(Actions.RandomOpponentMinion, Actions.RandomAmount(1, 4));

				// Create first layer of nodes underneath the root node
				// and add them to the search queue, then do breadth-first search
				var tree = RandomOutcomeSearch.Build(
					Game: game,
					SearchMode: new BreadthFirstActionWalker(),
					Action: () => {
						if (BoomBotTest) {
							var BoomBot = game.CurrentPlayer.Board.First(t => t.Card.Id == "GVG_110t") as Minion;
							BoomBot.Hit(1);
						}

						if (ArcaneMissilesTest) {
							ArcaneMissiles.Play();
						}
					}
				);

				// Print benchmark results
				Console.WriteLine("{0} branches in {1}ms", tree.NodeCount, sw.ElapsedMilliseconds);
				Console.WriteLine("{0} intermediate clones pruned ({1} unique branches kept)", tree.NodeCount - tree.LeafNodeCount, tree.LeafNodeCount);

				var uniqueGames = tree.GetUniqueGames().OrderBy(x => x.Key.Entities.FuzzyGameHash);
				sw.Stop();
				gotit = true;
				/*
				if (uniqueGames.Count() > -1) {
					gotit = true;
					foreach (var kv in uniqueGames) {
						gotit = true;
						Console.WriteLine(Math.Round(kv.Value * 100, 2) + "%: ");
						Console.WriteLine("{0:s}", kv.Key);
						foreach (var ph in kv.Key.PowerHistory.Skip(95))
							Console.WriteLine(ph);
						foreach (var aq in kv.Key.ActionQueue.History.Skip(99))
							Console.WriteLine(aq);
					}
				}*/
				Console.WriteLine("{0} unique games found in {1}ms", uniqueGames.Count(), sw.ElapsedMilliseconds);
			} while (!gotit);
			Console.ReadKey();
		}
	}
}
