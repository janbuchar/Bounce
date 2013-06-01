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
	}

	public Board createBoard (int width, int height)
	{
		int fieldSize = 20;
		BoardRenderer renderer = new BoardRenderer (this.canvas, width, height, fieldSize);
		board = new Board (width, height, fieldSize, renderer);
		return board;
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
