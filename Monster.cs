using System;

namespace Bounce
{
	public class Monster : Sprite
	{
		public string Type {
			get;
			protected set;
		}

		protected MonsterStrategy strategy;

		public Monster (MonsterStrategy strategy, string type, int x, int y) : base (x, y)
		{
			this.Type = type;
			this.strategy = strategy;
		}

		public void StartMove (NeighbourMap map, Board board)
		{
			StartMove (strategy.Move (map, board));
		}
	}
}

