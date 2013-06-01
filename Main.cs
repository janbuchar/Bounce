using System;
using Gtk;

namespace Bounce
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			LauncherWindow win = new LauncherWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
