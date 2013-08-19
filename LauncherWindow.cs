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
			win.StartGame (new Config (width.ValueAsInt, height.ValueAsInt, ballCount.ValueAsInt, monsterCount.ValueAsInt, lives.ValueAsInt, timePerBall.ValueAsInt));
			win.Show ();
			this.Hide ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}

