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

	protected Board createBoard (int width, int height)
	{
		int fieldSize = 20;
		BoardRenderer renderer = new BoardRenderer (this.canvas, width, height, fieldSize);
		board = new Board (width, height, fieldSize, renderer);
		return board;
	}

	public void StartGame (Config config)
	{
		Game game = new Game (this.createBoard(config.Width, config.Height));
		game.GameWon += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Jedeš vole!");
			dialog.Show ();
		};
		game.GameLost += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "No tak tos posral...");
			dialog.Show ();
		};
		game.FilledAreaChanged += delegate(object sender, int Value) {
			setFillCounter (Value);
		};
		game.LivesChanged += delegate(object sender, int Value) {
			setLifeCounter (Value);
		};
		game.Start (config);
	}

	protected void setLifeCounter (int value)
	{
		lifeCounter.Text = String.Format ("Životy: {0}", value);
	}

	protected void setFillCounter (int value)
	{
		fillCounter.Text = String.Format ("Zaplněno: {0}%", value);
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
