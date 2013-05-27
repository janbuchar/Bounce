using System;

namespace Bounce
{

	public class Player
	{
		public enum Direction
		{
			Up,
			Down,
			Right,
			Left
		}

		public Direction direction {
			get;
			protected set;
		}

		public int Remaining {
			get;
			protected set;
		}

		public int X {
			get;
			protected set;
		}

		public int Y {
			get;
			protected set;
		}

		public bool Moving {
			get;
			protected set;
		}

		public Player (int X, int Y)
		{
			this.X = X;
			this.Y = Y;
			Remaining = 0;
		}

		public void StartMove (Direction direction)
		{
			this.direction = direction;
			Moving = true;
		}

		public void Stop (int Remaining)
		{
			this.Remaining = Remaining;
			Moving = false;
		}

		public void Move (int steps)
		{
			if (Moving || Remaining > 0) {
				if (Remaining > 0) {
					Remaining -= steps;
				}
				switch (direction) {
				case Direction.Up:
					Y -= steps;
					break;
				case Direction.Down:
					Y += steps;
					break;
				case Direction.Left:
					X -= steps;
					break;
				case Direction.Right:
					X += steps;
					break;
				}
			}
		}
	}
}

