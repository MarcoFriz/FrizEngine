using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrizEngine.Utilities
{
	public interface IGameController
	{
		GameStates state{ get; }

		/// <summary>
		/// Is the moment previous to the game can start
		/// </summary>
		void PreStart ();

		/// <summary>
		/// Games the start.
		/// </summary>
		void GameStart ();

		/// <summary>
		/// Is the moment when the game is playing
		/// </summary>
		void Play ();

		/// <summary>
		/// Is the moment when the game is in Pause.
		/// </summary>
		void Pause ();

		/// <summary>
		/// Repeat o restart a level.
		/// </summary>
		void Repeat ();


		/// <summary>
		/// Is the moment preview to end of game
		/// </summary>
		void Stop ();

		/// <summary>
		/// Games the over.
		/// </summary>
		void GameOver ();
	}

	public enum GameStates
	{
		prestart,
		play,
		pause,
		stop,
		repeat,
		gameOver
	}
}