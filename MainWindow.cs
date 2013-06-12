using System;
using Gtk;
using Mono;
using Bounce;

public partial class MainWindow: Gtk.Window
{
	Board board;
	Game game;
	int level;

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
		game = new Game (this.createBoard (config.Width, config.Height));
		game.GameWon += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (
				this,
			 DialogFlags.Modal,
			 MessageType.Info,
			 ButtonsType.None,
			 "Jedeš vole!"
			);
			dialog.AddButton ("Další kolo", ResponseType.Accept);
			dialog.AddButton ("Konec hry", ResponseType.Cancel);
			dialog.Response += delegate(object o, ResponseArgs args) {
				if (args.ResponseId == ResponseType.Accept) {
					NextLevel (config);
				} else {
					Application.Quit ();
				}
			};
			dialog.Run ();
			dialog.Destroy ();
		};
		game.GameLost += delegate(object sender, EventArgs e) {
			MessageDialog dialog = new MessageDialog (
				this,
				DialogFlags.Modal,
				MessageType.Info,
				ButtonsType.None,
				"Tak tos posral..."
			);
			dialog.AddButton ("Nová hra", ResponseType.Accept);
			dialog.AddButton ("Konec", ResponseType.Close);
			dialog.Response += delegate(object o, ResponseArgs args) {
				if (args.ResponseId == ResponseType.Accept) {
					MainClass.ShowLauncher ();
					this.Destroy ();
				} else {
					Application.Quit ();
				}
			};
			dialog.Run ();
			dialog.Destroy ();
		};
		game.FilledAreaChanged += delegate(object sender, int value) {
			fillCounter.Text = String.Format ("Zaplněno: {0}%", value);
		};
		game.LivesChanged += delegate(object sender, int value) {
			lifeCounter.Text = String.Format ("Životy: {0}", value);
		};
		game.RemainingTimeChanged += delegate(object sender, int value) {
			remainingTimeCounter.Text = string.Format ("Zbývající čas: {0} sekund", value);
		};
		game.Start (config);
		level = 1;
		updateLevelCounter();
	}

	protected void PauseGame ()
	{
		game.Pause ();
		board.OverlayText = "Hra je pozastavena. Stiskněte libovolnou klávesu a pojede to.";
	}

	protected void NextLevel (Config config)
	{
		level += 1;
		updateLevelCounter ();
		config.BallCount += 1;
		config.Lives = game.Lives + 1;
		board.Clear ();
		game.Start (config);
	}

	protected void updateLevelCounter ()
	{
		levelCounter.Text = String.Format ("Úroveň: {0}", level);
	}

	protected void OnFocusOutEvent (object sender, FocusOutEventArgs args)
	{
		if (game.Running) {
			PauseGame ();
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCanvasExposeEvent (object o, ExposeEventArgs args)
	{
		if (game.Running) {
			board.Render ();
		}
	}

	protected void OnKeyPressEvent (object o, KeyPressEventArgs args)
	{
		if (!game.Running) {
			game.Resume ();
		} else {
			board.MovePlayer (keyToDirection(args.Event.Key));
		}
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
