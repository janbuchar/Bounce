using System;
using System.Collections.Generic;

namespace Bounce
{
	public class Wanderer : MonsterStrategy
	{
		Random random = new Random ();

		public Direction Move (NeighbourMap map, Board board)
		{
			Direction[] directions = new Direction[4];
			int peak = 0;

			if (map.Down != null && map.Down.Full) {
				directions [peak] = Direction.Down;
				peak += 1;
			}
			if (map.Up != null && map.Up.Full) {
				directions [peak] = Direction.Up;
				peak += 1;
			}
			if (map.Left != null && map.Left.Full) {
				directions [peak] = Direction.Left;
				peak += 1;
			}
			if (map.Right != null && map.Right.Full) {
				directions [peak] = Direction.Right;
				peak += 1;
			}

			return directions [random.Next (0, peak)];
		}
	}
}

