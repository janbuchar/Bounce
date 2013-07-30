using System;
using System.Collections.Generic;

namespace Bounce
{
	public class Sniffer : MonsterStrategy
	{
		const double moveRateStep = 0.95;
		double moveRate = 0;
		int wait = 0;
		Random random = RandomAccessor.Instance;
		Stack<Field> path;

		public Direction Move (NeighbourMap map, Board board)
		{
			if (moveRate > 0 && path.Count > 0 && random.NextDouble () < moveRate) {
				moveRate *= moveRateStep;
			} else {
				if (wait == 0) {
					sniff (map, board);
					wait = 30;
				} else {
					wait -= 1;
				}
				return Direction.None;
			}
			return getDirection (map.Current, path.Pop ());
		}

		protected void sniff (NeighbourMap map, Board board)
		{
			moveRate = moveRateStep;

			int[,] record = new int[board.Width, board.Height];
			for (int i = 0; i < record.GetLength(0); i++) {
				for (int j = 0; j < record.GetLength(1); j++) {
					record [i, j] = -1;
				}
			}
			record [map.Current.X, map.Current.Y] = 0;

			Queue<Field> queue = new Queue<Field> ();
			queue.Enqueue (map.Current);
			Field target = board.Player.BaseField;

			while (queue.Count > 0) {
				Field field = queue.Dequeue ();
				if (field == target) {
					break;
				}
				foreach (Field neighbour in board.GetNeighbours(field)) {
					if (neighbour.Full && record [neighbour.X, neighbour.Y] == -1) {
						record [neighbour.X, neighbour.Y] = record [field.X, field.Y] + 1;
						queue.Enqueue (neighbour);
					}
				}
			}
			
			path = new Stack<Field> ();
			path.Push (target);

			for (int i = record[target.X, target.Y] - 1; i > 0; i--) {
				foreach (Field neighbour in board.GetNeighbours(target)) {
					if (record [neighbour.X, neighbour.Y] == i) {
						path.Push (neighbour);
						target = neighbour;
						break;
					}
				}
			}
		}

		protected Direction getDirection (Field from, Field to)
		{
			if (from.X > to.X) {
				return Direction.Left;
			} else if (from.X < to.X) {
				return Direction.Right;
			} else if (from.Y > to.Y) {
				return Direction.Up;
			} else if (from.Y < to.Y) {
				return Direction.Down;
			}
			return Direction.None;
		}
	}
}

