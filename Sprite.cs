using System;

namespace Bounce
{
	public abstract class Sprite
	{
		public int X {
			get;
			protected set;
		}

		public int Y {
			get;
			protected set;
		}

		public Direction Direction {
			get;
			protected set;
		}

		public bool Moving {
			get;
			protected set;
		}

		public int Remaining {
			get;
			protected set;
		}

		public Sprite (int x, int y)
		{
			this.X = x;
			this.Y = y;
			Remaining = 0;
		}

		public void StartMove (Direction direction)
		{
			this.Direction = direction;
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
				switch (Direction) {
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

		public void Place (int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}

