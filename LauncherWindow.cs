using System;
using Gtk;

namespace Bounce
{
	public partial class LauncherWindow : Gtk.Window
	{
		public LauncherWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		protected void OnPlayButtonClicked (object sender, EventArgs e)
		{
			MainWindow win = new MainWindow ();
			Board board = win.CreateBoard (width.ValueAsInt, height.ValueAsInt);
			Game game = new Game (board);
			game.Start (new Config(ballCount.ValueAsInt));
			win.Show ();
			this.Destroy ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}

