
// This file has been generated by the GUI designer. Do not modify.
public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	private global::Gtk.DrawingArea canvas;
	private global::Gtk.Statusbar statusbar1;
	private global::Gtk.Label lifeCounter;
	private global::Gtk.Label fillCounter;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Bounce");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.canvas = new global::Gtk.DrawingArea ();
		this.canvas.Name = "canvas";
		this.vbox1.Add (this.canvas);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.canvas]));
		w1.Position = 0;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		// Container child statusbar1.Gtk.Box+BoxChild
		this.lifeCounter = new global::Gtk.Label ();
		this.lifeCounter.Name = "lifeCounter";
		this.lifeCounter.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
		this.statusbar1.Add (this.lifeCounter);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.statusbar1 [this.lifeCounter]));
		w2.Position = 2;
		w2.Expand = false;
		w2.Fill = false;
		// Container child statusbar1.Gtk.Box+BoxChild
		this.fillCounter = new global::Gtk.Label ();
		this.fillCounter.Name = "fillCounter";
		this.fillCounter.LabelProp = global::Mono.Unix.Catalog.GetString ("label2");
		this.statusbar1.Add (this.fillCounter);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.statusbar1 [this.fillCounter]));
		w3.Position = 3;
		w3.Expand = false;
		w3.Fill = false;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w4.Position = 1;
		w4.Expand = false;
		w4.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 400;
		this.DefaultHeight = 300;
		this.Show ();
		this.KeyPressEvent += new global::Gtk.KeyPressEventHandler (this.OnKeyPressEvent);
		this.KeyReleaseEvent += new global::Gtk.KeyReleaseEventHandler (this.OnKeyReleaseEvent);
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.canvas.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnCanvasExposeEvent);
	}
}
