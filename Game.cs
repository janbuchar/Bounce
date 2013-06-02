using System;

namespace Bounce
{
	public delegate void GameWonHandler (object sender, EventArgs e);
	public delegate void GameLostHandler (object sender, EventArgs e);
	public class Game
	{
		Board board;
		Random random = new Random ();
		protected int filled;
		protected uint timeoutID;
		const int victoryCondition = 70;

		public event GameWonHandler GameWon;
		public event GameLostHandler GameLost;

		public Game (Board board)
		{
			this.board = board;
			board.AreaFilled += delegate(object sender, AreaFilledEventArgs e) {
				filled += e.FilledArea;
				if (Math.Floor ((decimal)((100 * filled) / (board.Width * board.Height))) >= victoryCondition) {
					this.End ();
					if (GameWon != null) {
						GameWon (this, EventArgs.Empty);
					}
				}
			};
		}

		public void Start (Config config)
		{
			for (int i = 0; i < config.BallCount; i++) {
				spawnBall ();
			}
			timeoutID = GLib.Timeout.Add (40, delegate {
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

		public void End ()
		{
			GLib.Source.Remove (timeoutID);
		}
	}

	public class Config
	{
		public int Width, Height, BallCount;

		public Config (int Width = 0, int Height = 0, int BallCount = 0)
		{
			this.Width = Width;
			this.Height = Height;
			this.BallCount = BallCount;
		}
	}
}

