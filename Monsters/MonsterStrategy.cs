using System;
using System.Collections.Generic;

namespace Bounce
{
	public interface MonsterStrategy
	{
		Direction Move (NeighbourMap map);
	}
}

