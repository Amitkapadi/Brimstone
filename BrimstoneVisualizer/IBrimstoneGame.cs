﻿using Brimstone;

namespace BrimstoneVisualizer
{
	public interface IBrimstoneGame
	{
		Game SetupGame();
		void PlayGame(Game Game);
	}
}
