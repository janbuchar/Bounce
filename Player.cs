using System;
using System.Collections.Generic;

namespace Bounce
{
	public class Player
	{
		public Direction direction {
			get;
			protected set;
		}

		public Direction SteeringDirection;

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

		public Queue<Field> Trail = new Queue<Field> ();
		public Field BaseField;

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

		public void Steer ()
		{
			StartMove (SteeringDirection);
			SteeringDirection = Direction.None;
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

		public void Place (int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}

