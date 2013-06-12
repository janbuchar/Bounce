using System;
using Gtk;
using System.Collections.Generic;

namespace Bounce
{
	public class BoardRenderer
	{
		Gtk.DrawingArea area;
		int width, height, fieldSize;
		Cairo.Surface background;

		public BoardRenderer (Gtk.DrawingArea area, int width, int height, int fieldSize)
		{
			area.SetSizeRequest (width * fieldSize, height * fieldSize);
			this.area = area;
			this.width = width;
			this.height = height;
			this.fieldSize = fieldSize;
		}

		public void Render (Player player, List<Ball> balls)
		{
			Gdk.Window canvas = area.GdkWindow;
			if (canvas != null) {
				using (Cairo.Context context = Gdk.CairoHelper.Create (canvas)) {
					canvas.BeginPaintRegion (new Gdk.Region());
					context.SetSourceSurface (background, 0, 0);
					context.Paint ();

					foreach (Field field in player.Trail) {
						paintTrail (context, field.X * fieldSize, field.Y * fieldSize);
					}

					foreach (Ball ball in balls) {
						context.SetSourceRGB (1, 0, 0);		
						paintCircle (context, ball.X, ball.Y);
					}

					context.SetSourceRGB (0, 0, 1);
					paintPlayer (context, player);

					canvas.EndPaint ();
				}
			}
		}

		public void RenderOverlay (string text)
		{
			Gdk.Window canvas = area.GdkWindow;
			using (Cairo.Context context = Gdk.CairoHelper.Create(canvas)) {
				context.SetSourceRGBA (1, 1, 0, 0.5);
				context.Rectangle (new Cairo.Rectangle (0, 0, fieldSize * (width), fieldSize * (height)));
				context.Paint ();
				context.SetSourceRGBA (1, 1, 1, 1);
				Pango.Layout layout = Pango.CairoHelper.CreateLayout (context);
				layout.Width = (int)(width * fieldSize * Pango.Scale.PangoScale);
				layout.Alignment = Pango.Alignment.Center;
				layout.Wrap = Pango.WrapMode.Word;
				layout.FontDescription = Pango.FontDescription.FromString ("sans-serif 30");
				layout.SetText (text);
				int layoutWidth, layoutHeight;
				layout.GetPixelSize (out layoutWidth, out layoutHeight);
				context.MoveTo (0, (height * fieldSize - layoutHeight) / 2);
				Pango.CairoHelper.UpdateLayout (context, layout);
				Pango.CairoHelper.ShowLayout (context, layout);
			}
		}

		public void RefreshBackground (Field[,] fields)
		{
			background = new Cairo.ImageSurface (Cairo.Format.ARGB32, width * fieldSize, height * fieldSize);
			using (Cairo.Context context = new Cairo.Context(background)) {
				paintBackground (context, fields);
			}
		}

		protected void paintBackground (Cairo.Context context, Field[,] fields)
		{
			context.SetSourceRGB (0.8, 0.8, 0.8);
			context.Paint ();
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					paintSquare (context, i * fieldSize, j * fieldSize, fields [i, j].Full);
				}
			}
			paintGrid (context);
			context.Paint ();
		}

		protected void paintGrid (Cairo.Context context)
		{
			context.LineWidth = 0.3;
			context.SetSourceRGBA (0, 0, 0, 0.2);
			for (int i = 0; i < width; i++) {
				context.MoveTo (i * fieldSize, 0);
				context.LineTo (i * fieldSize, height * fieldSize);
				context.Stroke ();
				context.NewPath ();
			}
			for (int i = 0; i < height; i++) {
				context.MoveTo (0, i * fieldSize);
				context.LineTo (width * fieldSize, i * fieldSize);
				context.Stroke ();
				context.NewPath ();
			}
		}

		protected void paintSquare (Cairo.Context context, int x, int y, bool fill)
		{
			context.Save ();
			context.SetSourceRGB (0, 0, 1);
			context.Translate (x, y);
			context.Rectangle (new Cairo.Rectangle (0, 0, fieldSize, fieldSize));
			context.SetSourceRGBA (0, 0, 0, fill ? 0.5 : 0.3);
			context.FillPreserve ();
			context.NewPath ();
			context.Restore ();
		}

		protected void paintTrail (Cairo.Context context, int x, int y)
		{
			context.Save ();
			context.SetSourceRGB (0, 1, 0);
			context.Translate (x, y);
			context.Rectangle (new Cairo.Rectangle (0, 0, fieldSize, fieldSize));
			context.SetSourceRGBA (0, 1, 0, 0.3);
			context.FillPreserve ();
			context.NewPath ();
			context.Restore ();
		}

		protected void paintCircle (Cairo.Context context, int x, int y)
		{
			context.Arc (x + fieldSize / 2, y + fieldSize / 2, fieldSize / 2, 0, 2 * Math.PI);
			context.FillPreserve ();
			context.NewPath ();
		}

		protected void paintPlayer (Cairo.Context context, Player player)
		{
			context.Save ();
			context.Translate (player.X, player.Y);
			switch (player.direction) {
			case Direction.Down:
				context.Rotate (Math.PI);
				context.Translate (- fieldSize, - fieldSize);
				break;
			case Direction.Right:
				context.Rotate (Math.PI / 2);
				context.Translate (0, - fieldSize);
				break;
			case Direction.Left:
				context.Rotate (3 * Math.PI / 2);
				context.Translate (- fieldSize, 0);
				break;
			}
			context.MoveTo (fieldSize / 2, 0);
			context.LineTo (fieldSize, fieldSize);
			context.LineTo (fieldSize / 2, 3 * fieldSize / 4);
			context.LineTo (0, fieldSize);
			context.LineTo (fieldSize / 2, 0);
			context.SetSourceRGB (0, 0, 1);
			context.FillPreserve ();
			context.NewPath ();
			context.Restore ();
		}
	}
}

