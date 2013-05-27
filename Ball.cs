using System;

namespace Bounce
{
	public class Ball
	{
		public int X;

		public int Y;

		public int dX;

		public int dY;

		public Ball (int X, int Y, int dX, int dY)
		{
			this.X = X;
			this.Y = Y;
			this.dX = dX;
			this.dY = dY;
		}

		public void BounceX ()
		{
			dX = -dX;
		}

		public void BounceY ()
		{
			dY = -dY;
		}
	}
}

