using System;
using System.Collections.Generic;

namespace Bounce
{
	public class Player : Sprite
	{
		public Direction SteeringDirection;
		public Queue<Field> Trail = new Queue<Field> ();
		public Field BaseField;

		public Player (int x, int y): base (x, y)
		{
		}

		public void Steer ()
		{
			StartMove (SteeringDirection);
			SteeringDirection = Direction.None;
		}
	}
}

