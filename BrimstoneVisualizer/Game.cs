﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brimstone;

namespace BrimstoneVisualizer
{
	public partial class App
	{
		public static void PlayGame() {
			var p1 = Game.Player1;
			var p2 = Game.Player2;

			// Put a Piloted Shredder and Flame Juggler in each player's hand
			p1.Give(Cards.FindByName("Piloted Shredder"));
			p1.Give(Cards.FindByName("Flame Juggler"));
			p2.Give(Cards.FindByName("Piloted Shredder"));
			p2.Give(Cards.FindByName("Flame Juggler"));

			// Fill the board with Flame Jugglers
			for (int i = 0; i < MaxMinions; i++) {
				var fj = p1.Give(Cards.FindByName("Flame Juggler"));
				fj.Play();
			}

			Game.BeginTurn();

			for (int i = 0; i < MaxMinions; i++) {
				var fj = p2.Give(Cards.FindByName("Flame Juggler"));
				fj.Play();
			}
		}
	}
}
