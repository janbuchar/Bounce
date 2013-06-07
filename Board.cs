using System;
using System.Collections.Generic;
using Gtk;
using Mono;

namespace Bounce
{
	public class AreaFilledEventArgs : EventArgs
	{
		public int FilledArea;

		public AreaFilledEventArgs (int filled)
		{
			FilledArea = filled;
		}
	}
	public delegate void AreaFilledHandler (object sender, AreaFilledEventArgs e);
	public delegate void PlayerCollisionHandler (object sender, EventArgs e);
	public class Board
	{
		protected Player player = new Player (0, 0);
		protected List<Ball> balls = new List<Ball> ();
		private Field[,] fields;
		int fieldSize;
		BoardRenderer renderer;

		public int Width { get; protected set; }

		public int Height { get; protected set; }

		public event AreaFilledHandler AreaFilled;
		public event PlayerCollisionHandler PlayerCollision;

		public Board (int width, int height, int fieldSize, BoardRenderer renderer)
		{
			this.renderer = renderer;
			this.Width = width;
			this.Height = height;
			this.Clear ();
			this.fieldSize = fieldSize;
			renderer.RefreshBackground (fields);
		}

		public void Clear ()
		{
			player = new Player (0, 0);
			balls = new List<Ball> ();
			fields = new Field[Width, Height];
			for (int i = 0; i < Width; i++) {
				for (int j = 0; j < Height; j++) {
					fields [i, j] = new Field ();
					if (i == 0 || j == 0 || i + 1 == Width || j + 1 == Height) {
						fields [i, j].Full = true;
					} else {
						fields [i, j].Full = false;
					}
					fields [i, j].X = i;
					fields [i, j].Y = j;
				}
			}
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

		protected void closeTrail ()
		{
			int filled = 0;
			List<Field> ballMap = new List<Field> ();
			foreach (Ball ball in balls) {
				ballMap.Add (crossedField (ball.X, ball.Y));
			}
			while (player.Trail.Count > 0) {
				player.Trail.Dequeue ().Full = true;
				filled += 1;
			}
			bool[,] visited = new bool[Width, Height]; 

			List<Field> reserve = new List<Field> ();
			do {
				reserve = calculateReserve (ballMap, visited);
				foreach (Field field in reserve) {
					field.Full = true;
					filled += 1;
				}
			} while (!isFull(visited));
			renderer.RefreshBackground (fields);
			if (AreaFilled != null) {
				AreaFilled (this, new AreaFilledEventArgs (filled));
			}
		}

		private bool isFull (bool[,] value)
		{
			for (int i = 0; i < value.GetLength(0); i++) {
				for (int j = 0; j < value.GetLength(1); j++) {
					if (!value [i, j] && !fields [i, j].Full) {
						return false;
					}
				}
			}
			return true;
		}

		protected List<Field> calculateReserve (List<Field> ballMap, bool[,] visited)
		{
			Field start = null;
			bool containsBall = false;
			List<Field> result = new List<Field> ();
			for (int i = 0; i < Width; i++) {
				for (int j = 0; j < Height; j++) {
					if (!fields [i, j].Full && !visited [i, j]) {
						start = fields [i, j];
						visited [i, j] = true;
						result.Add (start);
						break;
					}
				}
				if (start != null) {
					break;
				}
			}
			if (start != null) {
				Queue<Field> queue = new Queue<Field> ();
				queue.Enqueue (start);
				while (queue.Count > 0) {
					Field field = queue.Dequeue ();
					if (ballMap.Contains (field)) {
						containsBall = true;
					}
					foreach (Field neighbour in getNeighbours(field)) {
						if (!neighbour.Full && !visited [neighbour.X, neighbour.Y]) {
							result.Add (neighbour);
							queue.Enqueue (neighbour);
						}
						visited [neighbour.X, neighbour.Y] = true;
					}
				}
			}
			if (containsBall) {
				result.Clear ();
			}
			return result;
		}

		protected IEnumerable<Field> getNeighbours (Field field)
		{
			if (field.X > 0) {
				yield return fields [field.X - 1, field.Y];
			}
			if (field.X + 1 < Width) {
				yield return fields [field.X + 1, field.Y];
			}
			if (field.Y > 0) {
				yield return fields [field.X, field.Y - 1];
			}
			if (field.Y + 1 < Height) {
				yield return fields [field.X, field.Y + 1];
			}
		}

		public void MoveBalls ()
		{
			foreach (Ball ball in balls) {
				for (int i = 0; i < 5; i++) {
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
					if (player.Trail.Contains (crossedField(ball.X, ball.Y))) {
						player.Trail.Clear ();
						player.Place (player.BaseField.X * fieldSize, player.BaseField.Y * fieldSize);
						if (PlayerCollision != null) {
							PlayerCollision (this, EventArgs.Empty);
						}
					}
				}
			}

			if (player.Moving) {
				int steps = checkedPlayerDistance (5);
				player.Move (steps);
				if (steps == 0) {
					player.Stop (0);
				}
			}
			if (player.Remaining > 0) {
				player.Move (Math.Min (5, player.Remaining));
				if (player.Remaining == 0 && player.SteeringDirection != Player.Direction.None) {
					player.Steer ();
				}
			}
			Field playerField = crossedField (player.X, player.Y);
			if (!playerField.Full && !player.Trail.Contains (playerField)) {
				player.Trail.Enqueue (playerField);
			} else if (playerField.Full) {
				if (player.Trail.Count > 0) {
					closeTrail ();
				}
				player.BaseField = playerField;
			}
		}

		protected Field crossedField (int x, int y)
		{
			return fields [x / fieldSize, y / fieldSize];
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

		public int calculateResidualSteps ()
		{
			switch (player.direction) {
			case Player.Direction.Up:
				return player.Y % fieldSize;
			case Player.Direction.Down:
				return fieldSize - player.Y % fieldSize;
			case Player.Direction.Right:
				return fieldSize - player.X % fieldSize;
			case Player.Direction.Left:
				return player.X % fieldSize;
			default:
				return 0;
			}
		}

		public void MovePlayer (Player.Direction direction)
		{
			if (!player.Moving && player.Remaining == 0) {
				player.StartMove (direction);
			} else {
				if (player.Moving) {
					StopPlayer (player.direction);
				}
				player.SteeringDirection = direction;
			}
		}

		public void StopPlayer (Player.Direction direction)
		{
			if (direction != Player.Direction.None) {
				if (direction == player.direction) {
					player.Stop (checkedPlayerDistance(calculateResidualSteps()));
				}
				if (direction == player.SteeringDirection) {
					player.SteeringDirection = Player.Direction.None;
				}
			}
		}
	}
}

