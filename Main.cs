using System;
using Gtk;

namespace Bounce
{
	class MainClass
	{
		static LauncherWindow launcher;

		public static void Main (string[] args)
		{
			Application.Init ();
			launcher = new LauncherWindow ();
			launcher.Show ();
			Application.Run ();
		}

		public static void ShowLauncher ()
		{
			launcher.Show ();
		}
	}
}
