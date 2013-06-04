
// This file has been generated by the GUI designer. Do not modify.
namespace Bounce
{
	public partial class LauncherWindow
	{
		private global::Gtk.VBox vbox2;
		private global::Gtk.Table table1;
		private global::Gtk.SpinButton ballCount;
		private global::Gtk.Label ballCountLabel;
		private global::Gtk.SpinButton height;
		private global::Gtk.Label heightLabel;
		private global::Gtk.SpinButton lives;
		private global::Gtk.Label livesLabel;
		private global::Gtk.SpinButton width;
		private global::Gtk.Label widthLabel;
		private global::Gtk.Button playButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Bounce.LauncherWindow
			this.Name = "Bounce.LauncherWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("LauncherWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child Bounce.LauncherWindow.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(4)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.ballCount = new global::Gtk.SpinButton (0, 100, 1);
			this.ballCount.CanFocus = true;
			this.ballCount.Name = "ballCount";
			this.ballCount.Adjustment.PageIncrement = 10;
			this.ballCount.ClimbRate = 1;
			this.ballCount.Numeric = true;
			this.ballCount.Value = 2;
			this.table1.Add (this.ballCount);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this.ballCount]));
			w1.TopAttach = ((uint)(2));
			w1.BottomAttach = ((uint)(3));
			w1.LeftAttach = ((uint)(1));
			w1.RightAttach = ((uint)(2));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ballCountLabel = new global::Gtk.Label ();
			this.ballCountLabel.Name = "ballCountLabel";
			this.ballCountLabel.Xalign = 1F;
			this.ballCountLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Počet koulí");
			this.table1.Add (this.ballCountLabel);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.ballCountLabel]));
			w2.TopAttach = ((uint)(2));
			w2.BottomAttach = ((uint)(3));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.height = new global::Gtk.SpinButton (0, 100, 1);
			this.height.CanFocus = true;
			this.height.Name = "height";
			this.height.Adjustment.PageIncrement = 10;
			this.height.ClimbRate = 1;
			this.height.Numeric = true;
			this.height.Value = 20;
			this.table1.Add (this.height);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.height]));
			w3.TopAttach = ((uint)(1));
			w3.BottomAttach = ((uint)(2));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.heightLabel = new global::Gtk.Label ();
			this.heightLabel.Name = "heightLabel";
			this.heightLabel.Xalign = 1F;
			this.heightLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Výška hrací plochy");
			this.table1.Add (this.heightLabel);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.heightLabel]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lives = new global::Gtk.SpinButton (0, 100, 1);
			this.lives.CanFocus = true;
			this.lives.Name = "lives";
			this.lives.Adjustment.PageIncrement = 10;
			this.lives.ClimbRate = 1;
			this.lives.Numeric = true;
			this.lives.Value = 5;
			this.table1.Add (this.lives);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.lives]));
			w5.TopAttach = ((uint)(3));
			w5.BottomAttach = ((uint)(4));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.livesLabel = new global::Gtk.Label ();
			this.livesLabel.Name = "livesLabel";
			this.livesLabel.Xalign = 1F;
			this.livesLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Počet životů");
			this.table1.Add (this.livesLabel);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.livesLabel]));
			w6.TopAttach = ((uint)(3));
			w6.BottomAttach = ((uint)(4));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.width = new global::Gtk.SpinButton (0, 100, 1);
			this.width.CanFocus = true;
			this.width.Name = "width";
			this.width.Adjustment.PageIncrement = 10;
			this.width.ClimbRate = 1;
			this.width.Numeric = true;
			this.width.Value = 30;
			this.table1.Add (this.width);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.width]));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.widthLabel = new global::Gtk.Label ();
			this.widthLabel.Name = "widthLabel";
			this.widthLabel.Xalign = 1F;
			this.widthLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Šířka hrací plochy");
			this.table1.Add (this.widthLabel);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.widthLabel]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add (this.table1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.table1]));
			w9.Position = 0;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.playButton = new global::Gtk.Button ();
			this.playButton.CanFocus = true;
			this.playButton.Name = "playButton";
			this.playButton.UseUnderline = true;
			this.playButton.Label = global::Mono.Unix.Catalog.GetString ("Hrát");
			this.vbox2.Add (this.playButton);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.playButton]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			this.Add (this.vbox2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.playButton.Clicked += new global::System.EventHandler (this.OnPlayButtonClicked);
		}
	}
}
