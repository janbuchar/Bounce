using System;

namespace Bounce
{
	public static class RandomAccessor
	{
		private static Random instance;

		public static Random Instance {
			get {
				if (instance == null) {
					instance = new Random ();
				}
				return instance;
			}
		}
	}
}

