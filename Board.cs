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
		protected List<Ball> balls = new List<Ball> ();
		protected List<Monster> monsters = new List<Monster> ();
		private Field[,] fields;
		int fieldSize;
		BoardRenderer renderer;

		public string OverlayText {
			set {
				renderer.RenderOverlay (value);
			}
		}

		public Player Player {
			get;
			protected set;
		}

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
			Player = new Player (0, 0);
			balls = new List<Ball> ();
			monsters = new List<Monster> ();
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
			renderer.Render (Player, balls, monsters);
		}

		public void AddBall (int x, int y, int dX, int dY)
		{
			balls.Add (new Ball(x * fieldSize, y * fieldSize, dX, dY));
		}

		public void AddMonster (string type, int x, int y)
		{
			MonsterStrategy strategy = (MonsterStrategy)Activator.CreateInstance (Type.GetType("Bounce." + type));
			monsters.Add (new Monster(strategy, type, x * fieldSize, y * fieldSize));
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
			while (Player.Trail.Count > 0) {
				Player.Trail.Dequeue ().Full = true;
				filled += 1;
			}
			bool[,] visited = new bool[Width, Height]; 

			List<Field> reserve = new List<Field> ();
			
			Func<bool> visitedAll = (delegate () {
				for (int i = 0; i < visited.GetLength(0); i++) {
					for (int j = 0; j < visited.GetLength(1); j++) {
						if (!visited [i, j] && !fields [i, j].Full) {
							return false;
						}
					}
				}
				return true;
			});
			
			do {
				reserve = calculateReserve (ballMap, visited);
				foreach (Field field in reserve) {
					field.Full = true;
					filled += 1;
				}
			} while (!visitedAll());
			
			renderer.RefreshBackground (fields);
			if (AreaFilled != null) {
				AreaFilled (this, new AreaFilledEventArgs (filled));
			}
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
					foreach (Field neighbour in GetNeighbours(field)) {
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

		public IEnumerable<Field> GetNeighbours (Field field)
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
					if (Player.Trail.Contains (crossedField(ball.X, ball.Y))) {
						Player.Trail.Clear ();
						Player.Place (Player.BaseField.X * fieldSize, Player.BaseField.Y * fieldSize);
						if (PlayerCollision != null) {
							PlayerCollision (this, EventArgs.Empty);
						}
					}
				}
			}

			foreach (Monster monster in monsters) {
				if (monster.Remaining == 0) {
					Field field = crossedField (monster.X, monster.Y);
					monster.StartMove (new NeighbourMap(field, GetNeighbours(field)), this);
					monster.Move (checkedSpriteDistance(monster, 5));
					monster.Stop (checkedSpriteDistance(monster, calculateResidualSteps(monster)));
				} else {
					monster.Move (Math.Min (5, monster.Remaining));
				}
			}

			if (Player.Moving) {
				int steps = checkedSpriteDistance (Player, 5);
				Player.Move (steps);
				if (steps == 0) {
					Player.Stop (0);
				}
			}
			if (Player.Remaining > 0) {
				Player.Move (Math.Min (5, Player.Remaining));
				if (Player.Remaining == 0 && Player.SteeringDirection != Direction.None) {
					Player.Steer ();
				}
			}
			Field playerField = crossedField (Player.X, Player.Y);
			if (!playerField.Full && !Player.Trail.Contains (playerField)) {
				Player.Trail.Enqueue (playerField);
			} else if (playerField.Full) {
				if (Player.Trail.Count > 0) {
					closeTrail ();
				}
				Player.BaseField = playerField;
			}
		}

		protected Field crossedField (int x, int y)
		{
			return fields [x / fieldSize, y / fieldSize];
		}

		public int checkedSpriteDistance (Sprite sprite, int steps)
		{
			int max = 0;
			switch (sprite.Direction) {
			case Direction.Down:
				max = Height * fieldSize - (sprite.Y + fieldSize);
				break;
			case Direction.Up:
				max = sprite.Y;
				break;
			case Direction.Right:
				max = Width * fieldSize - (sprite.X + fieldSize);
				break;
			case Direction.Left:
				max = sprite.X;
				break;
			}
			return Math.Min (steps, max);
		}

		public int calculateResidualSteps (Sprite sprite)
		{
			switch (sprite.Direction) {
			case Direction.Up:
				return sprite.Y % fieldSize;
			case Direction.Down:
				return fieldSize - sprite.Y % fieldSize;
			case Direction.Right:
				return fieldSize - sprite.X % fieldSize;
			case Direction.Left:
				return sprite.X % fieldSize;
			default:
				return 0;
			}
		}

		public void MovePlayer (Direction direction)
		{
			if (!Player.Moving && Player.Remaining == 0) {
				Player.StartMove (direction);
			} else {
				if (Player.Moving) {
					StopPlayer (Player.Direction);
				}
				Player.SteeringDirection = direction;
			}
		}

		public void StopPlayer (Direction direction)
		{
			if (direction != Direction.None) {
				if (direction == Player.Direction) {
					Player.Stop (checkedSpriteDistance(Player, calculateResidualSteps(Player)));
				}
				if (direction == Player.SteeringDirection) {
					Player.SteeringDirection = Direction.None;
				}
			}
		}
	}

	public class NeighbourMap
	{
		public Field Current {
			get;
			protected set;
		}

		public Field Left {
			get;
			protected set;
		}

		public Field Up {
			get;
			protected set;
		}

		public Field Right {
			get;
			protected set;
		}

		public Field Down {
			get;
			protected set;
		}

		public NeighbourMap (Field current, IEnumerable<Field> neighbours)
		{
			this.Current = current;
			foreach (Field neighbour in neighbours) {
				if (neighbour.X == current.X - 1) {
					Left = neighbour;
				}
				if (neighbour.X == current.X + 1) {
					Right = neighbour;
				}
				if (neighbour.Y == current.Y - 1) {
					Up = neighbour;
				}
				if (neighbour.Y == current.Y + 1) {
					Down = neighbour;
				}
			}
		}
	}
}

