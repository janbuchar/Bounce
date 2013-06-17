using System;

namespace Bounce
{
	public class Circulator : MonsterStrategy
	{
		Direction direction = Direction.None;

		public Direction Move (NeighbourMap map)
		{
			if (direction == Direction.None && !checkPath (map)) {
				turnAnywhere (map);
			} else {
				if (rightHand (map) != null && !rightHand (map).Full) {
					while (front(map) == null || !front (map).Full) {
						turnLeft ();
					}
				} else {
					turnRight ();
				}
			}

			return direction;
		}

		protected bool checkPath (NeighbourMap map)
		{
			if ((map.Left == null || map.Left.Full) && 
				(map.Right == null || map.Right.Full) && 
				(map.Up == null || map.Up.Full) && 
				(map.Down == null || map.Down.Full)) {
				return false;
			}
			return true;
		}

		protected void turnAnywhere (NeighbourMap map)
		{
			if (map.Left != null && map.Left.Full) {
				direction = Direction.Left;
			} else if (map.Up != null && map.Up.Full) {
				direction = Direction.Up;
			} else if (map.Right != null && map.Right.Full) {
				direction = Direction.Right;
			} else if (map.Down != null && map.Down.Full) {
				direction = Direction.Down;
			} else {
				direction = Direction.None;
			}
		}

		protected void turnLeft ()
		{
			switch (direction) {
			case Direction.Down:
				direction = Direction.Right;
				break;
			case Direction.Right:
				direction = Direction.Up;
				break;
			case Direction.Left:
				direction = Direction.Down;
				break;
			case Direction.Up:
				direction = Direction.Left;
				break;
			}
		}

		protected void turnRight ()
		{
			switch (direction) {
			case Direction.Down:
				direction = Direction.Left;
				break;
			case Direction.Right:
				direction = Direction.Down;
				break;
			case Direction.Left:
				direction = Direction.Up;
				break;
			case Direction.Up:
				direction = Direction.Right;
				break;
			}
		}

		protected Field front (NeighbourMap map)
		{
			switch (direction) {
			case Direction.Down:
				return map.Down;
			case Direction.Up:
				return map.Up;
			case Direction.Left:
				return map.Left;
			case Direction.Right:
				return map.Right;
			}
			return map.Current;
		}

		protected Field rightHand (NeighbourMap map)
		{
			switch (direction) {
			case Direction.Down:
				return map.Left;
			case Direction.Left:
				return map.Up;
			case Direction.Right:
				return map.Down;
			case Direction.Up:
				return map.Right;
			}
			return map.Current;
		}
	}
}

