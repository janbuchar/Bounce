using System;

namespace Bounce
{
	public class Game
	{
		Board board;
		Random random = new Random ();

		public Game (Board board)
		{
			this.board = board;
		}

		public void Start (Config config)
		{
			for (int i = 0; i < config.BallCount; i++) {
				spawnBall ();
			}
			GLib.Timeout.Add (40, delegate {
				board.MoveBalls ();
				board.Render ();
				return true;
			});
		}

		protected void spawnBall ()
		{
			board.AddBall (
				random.Next (2, board.Width), 
				random.Next (2, board.Height), 
				(int)Math.Pow (-1, random.Next (0, 2)), 
				(int)Math.Pow (-1, random.Next (0, 2))
			);
		}
	}

	public class Config
	{
		public int BallCount;

		public Config (int BallCount = 0)
		{
			this.BallCount = BallCount;
		}
	}
}

