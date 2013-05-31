using System;
using Gtk;
using Mono;
using Bounce;

public partial class MainWindow: Gtk.Window
{
	Board board;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		board = new Board (30, 20, this.canvas);
		board.Fill (10, 5);
		board.Fill (11, 6);
		board.Fill (12, 7);
		board.Fill (5, 7);
		board.Fill (3, 8);
		board.Fill (17, 3);
		board.AddBall (new Ball (60, 60, 1, 1));
		board.AddBall (new Ball (120, 80, -1, 1));
		GLib.Timeout.Add (40, delegate {
			board.MoveBalls ();
			board.Render ();
			return true;
		});
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCanvasExposeEvent (object o, ExposeEventArgs args)
	{
		board.Render ();
	}

	protected void OnKeyPressEvent (object o, KeyPressEventArgs args)
	{
		switch (args.Event.Key) {
		case Gdk.Key.w:
			board.MovePlayer (Player.Direction.Up);
			break;
		case Gdk.Key.s:
			board.MovePlayer (Player.Direction.Down);
			break;
		case Gdk.Key.a:
			board.MovePlayer (Player.Direction.Left);
			break;
		case Gdk.Key.d:
			board.MovePlayer (Player.Direction.Right);
			break;
		}
	}

	protected void OnKeyReleaseEvent (object o, KeyReleaseEventArgs args)
	{
		board.StopPlayer ();
	}
}
