using System;

namespace Bounce
{
	public delegate void GameWonHandler (object sender, EventArgs e);
	public delegate void GameLostHandler (object sender, EventArgs e);
	public delegate void LivesChangedHandler (object sender, int value);
	public delegate void FilledAreaChangedHandler (object sender, int value);
	public delegate void RemainingTimeChangedHandler (object sender, int value);
	public class Game
	{
		Board board;
		Random random = new Random ();

		public int Filled {
			get;
			protected set;
		}

		public int Lives {
			get;
			protected set;
		}

		public bool Running {
			get;
			protected set;
		}

		protected uint renderTimeoutID;
		protected uint limitTimeoutID;
		int passed, limit;
		const int victoryCondition = 70;

		public event GameWonHandler GameWon;
		public event GameLostHandler GameLost;
		public event LivesChangedHandler LivesChanged;
		public event FilledAreaChangedHandler FilledAreaChanged;
		public event RemainingTimeChangedHandler RemainingTimeChanged;

		public Game (Board board)
		{
			this.board = board;
			board.AreaFilled += delegate(object sender, AreaFilledEventArgs e) {
				Filled += e.FilledArea;
				if (FilledAreaChanged != null) {
					FilledAreaChanged (this, getFilledPercents ());
				}
				if (getFilledPercents () >= victoryCondition) {
					this.End ();
					if (GameWon != null) {
						GameWon (this, EventArgs.Empty);
					}
				}
			};
			board.PlayerCollision += delegate(object sender, EventArgs e) {
				Lives -= 1;
				if (LivesChanged != null) {
					LivesChanged (this, Lives);
				}
				if (Lives == 0) {
					this.End ();
					if (GameLost != null) {
						GameLost (this, EventArgs.Empty);
					}
				}
			};
			Running = false;
		}

		public void Start (Config config)
		{
			Filled = 2 * board.Width + 2 * board.Height - 4;
			if (FilledAreaChanged != null) {
				FilledAreaChanged (this, getFilledPercents ());
			}
			Lives = config.Lives;
			if (LivesChanged != null) {
				LivesChanged (this, Lives);
			}
			for (int i = 0; i < config.BallCount; i++) {
				spawnBall ();
			}
			for (int i = 0; i < config.MonsterCount; i++) {
				spawnMonster ();
			}

			startRenderTimeout ();
			
			limit = config.TimePerBall * config.BallCount;
			if (RemainingTimeChanged != null) {
				RemainingTimeChanged (this, limit);
			}
			passed = 0;
			
			startLimitTimeout ();
			Running = true;
		}

		public void End ()
		{
			board.MoveBalls ();
			board.Render ();
			GLib.Source.Remove (renderTimeoutID);
			GLib.Source.Remove (limitTimeoutID);
			Running = false;
		}

		public void Pause ()
		{
			GLib.Source.Remove (renderTimeoutID);
			GLib.Source.Remove (limitTimeoutID);
			Running = false;
		}

		public void Resume ()
		{
			Running = true;
			startRenderTimeout ();
			startLimitTimeout ();
		}

		protected void startRenderTimeout ()
		{
			renderTimeoutID = GLib.Timeout.Add (40, delegate {
				for (int i = 0; i < 5; i++) {
					board.MoveBalls ();
				}
				for (int i = 0; i < 5; i++) {
					board.MoveMonsters ();
				}
				for (int i = 0; i < 5; i++) {
					board.MovePlayer ();
				}
				board.Render ();
				return true;
			});
		}

		protected void startLimitTimeout ()
		{
			DateTime start = DateTime.Now;
			int initPassed = passed;
			limitTimeoutID = GLib.Timeout.Add (200, delegate {
				int newPassed = initPassed + (int)(DateTime.Now - start).TotalSeconds;
				if (newPassed > passed) {
					passed = newPassed;
					if (RemainingTimeChanged != null) {
						RemainingTimeChanged (this, Math.Max (limit - passed, 0));
					}
					if (passed > limit && GameLost != null) {
						End ();
						GameLost (this, EventArgs.Empty);
					}
				}
				return true;
			});
		}

		protected int getFilledPercents ()
		{
			return (int)Math.Floor ((decimal)((100 * Filled) / (board.Width * board.Height)));
		}

		protected void spawnBall ()
		{
			board.AddBall (
				random.Next (2, board.Width - 1), 
				random.Next (2, board.Height - 1), 
				(int)Math.Pow (-1, random.Next (0, 2)), 
				(int)Math.Pow (-1, random.Next (0, 2))
			);
		}

		protected void spawnMonster ()
		{
			string[] types = new string[] {
				"Wanderer",
				"Circulator",
				"Sniffer"
			};
			board.AddMonster (types [random.Next (0, types.Length)]);
		}
	}

	public class Config
	{
		public int Width, Height, BallCount, MonsterCount, Lives, TimePerBall;

		public Config (int Width = 0, int Height = 0, int BallCount = 0, int MonsterCount = 0, int Lives = 0, int TimePerBall = 0)
		{
			this.Width = Width;
			this.Height = Height;
			this.BallCount = BallCount;
			this.MonsterCount = MonsterCount;
			this.Lives = Lives;
			this.TimePerBall = TimePerBall;
		}
	}
}

