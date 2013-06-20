using System;
using System.Collections.Generic;

namespace Bounce
{
	public class Wanderer : MonsterStrategy
	{
		Random random = new Random ();

		public Direction Move (NeighbourMap map, Board board)
		{
			switch (random.Next (1, 6)) {
			case 1:
				if (map.Down != null && map.Down.Full) {
					return Direction.Down;
				}
				break;
			case 2:
				if (map.Left != null && map.Left.Full) {
					return Direction.Left;
				}
				break;
			case 3:
				if (map.Right != null && map.Right.Full) {
					return Direction.Right;
				}
				break;
			case 4:
				if (map.Up != null && map.Up.Full) {
					return Direction.Up;
				}
				break;
			}
			return Direction.None;
		}
	}
}

