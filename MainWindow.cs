using System;
using Gtk;
using Mono;
using Bounce;

public partial class MainWindow: Gtk.Window
{
	Board board;
	Game game;

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
		game = new Game (this.createBoard(config.Width, config.Height));
		game.GameWon += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Jedeš vole!");
			dialog.Run ();
			dialog.Destroy ();
			NextLevel (config);
		};
		game.GameLost += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "No tak tos posral...");
			dialog.Run ();
			dialog.Destroy ();
		};
		game.FilledAreaChanged += delegate(object sender, int Value) {
			setFillCounter (Value);
		};
		game.LivesChanged += delegate(object sender, int Value) {
			setLifeCounter (Value);
		};
		game.Start (config);
	}

	protected void NextLevel (Config config)
	{
		config.BallCount += 1;
		config.Lives += 1;
		board.Clear ();
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
		board.MovePlayer (keyToDirection(args.Event.Key));
	}

	protected void OnKeyReleaseEvent (object o, KeyReleaseEventArgs args)
	{
		board.StopPlayer (keyToDirection(args.Event.Key));
	}

	protected Player.Direction keyToDirection (Gdk.Key key)
	{
		switch (key) {
		case Gdk.Key.w:
			return (Player.Direction.Up);
		case Gdk.Key.s:
			return (Player.Direction.Down);
		case Gdk.Key.a:
			return (Player.Direction.Left);
		case Gdk.Key.d:
			return (Player.Direction.Right);
		default:
			return Player.Direction.None;
		}
	}
}
