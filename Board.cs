using System;
using System.Collections.Generic;
using Gtk;
using Mono;

namespace Bounce
{
	public class Board
	{
		protected Player player = new Player (0, 0);
		protected List<Ball> balls = new List<Ball> ();
		private Field[,] fields;
		int fieldSize;
		BoardRenderer renderer;

		public int Width { get; protected set; }

		public int Height { get; protected set; }

		public Board (int width, int height, int fieldSize, BoardRenderer renderer)
		{
			fields = new Field[width, height];
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					if (i == 0 || j == 0 || i + 1 == width || j + 1 == height) {
						fields [i, j].Full = true;
					} else {
						fields [i, j].Full = false;
					}
				}
			}
			this.renderer = renderer;
			this.Width = width;
			this.Height = height;
			this.fieldSize = fieldSize;
			renderer.RefreshBackground (fields);
		}

		public void Render ()
		{
			renderer.Render (player, balls);
		}

		public void AddBall (int x, int y, int dX, int dY)
		{
			balls.Add (new Ball(x * fieldSize, y * fieldSize, dX, dY));
		}

		public void Fill (int x, int y)
		{
			fields [x, y].Full = true;
			renderer.RefreshBackground (fields);
		}

		public void MoveBalls ()
		{
			foreach (Ball ball in balls) {
				for (int i = 0; i < 2; i++) {
					int posX = (ball.X + fieldSize / 2) / fieldSize;
					int posY = (ball.Y + ball.dY) / fieldSize;
					if (fields [posX, posY + 1].Full && ball.dY > 0) {
						ball.BounceY ();
					}
					if (fields [posX, posY].Full && ball.dY < 0) {
						ball.BounceY ();
					}
					posX = (ball.X + ball.dX) / fieldSize;
					posY = (ball.Y + fieldSize / 2) / fieldSize;
					if (fields [posX + 1, posY].Full && ball.dX > 0) {
						ball.BounceX ();
					}
					if (fields [posX, posY].Full && ball.dX < 0) {
						ball.BounceX ();
					}
					ball.X += ball.dX;
					ball.Y += ball.dY;	
				}
			}
			if (player.Moving) {
				player.Move (checkedPlayerDistance(5));
			}
			if (player.Remaining > 0) {
				player.Move (Math.Min (5, player.Remaining));
				if (player.Remaining == 0 && player.SteeringDirection != Player.Direction.None) {
					player.StartMove (player.SteeringDirection);
				}
			}
		}

		public int checkedPlayerDistance (int steps)
		{
			int max = 0;
			switch (player.direction) {
			case Player.Direction.Down:
				max = Height * fieldSize - (player.Y + fieldSize);
				break;
			case Player.Direction.Up:
				max = player.Y;
				break;
			case Player.Direction.Right:
				max = Width * fieldSize - (player.X + fieldSize);
				break;
			case Player.Direction.Left:
				max = player.X;
				break;
			}
			return Math.Min (steps, max);
		}

		public void MovePlayer (Player.Direction direction)
		{
			if (!player.Moving && player.Remaining == 0) {
				player.StartMove (direction);
			} else {
				if (player.Moving) {
					StopPlayer ();
				}
				player.SteeringDirection = direction;
			}
		}

		public void StopPlayer ()
		{
			int steps = 0;
			switch (player.direction) {
			case Player.Direction.Up:
				steps = player.Y % fieldSize;
				break;
			case Player.Direction.Down:
				steps = fieldSize - player.Y % fieldSize;
				break;
			case Player.Direction.Right:
				steps = fieldSize - player.X % fieldSize;
				break;
			case Player.Direction.Left:
				steps = player.X % fieldSize;
				break;
			}
			player.Stop (checkedPlayerDistance(steps));
		}
	}
}

