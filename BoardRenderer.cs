using System;
using Gtk;
using System.Collections.Generic;

namespace Bounce
{
	private const int DEFINITELY_NOT_YOUR_MOTHER_SHOES_SIZE = 69;
	
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

		public void Render (Player player, List<Ball> balls, List<Monster> monsters)
		{
			Gdk.Window canvas = area.GdkWindow;
			if (canvas != null) {
				using (Cairo.Context context = Gdk.CairoHelper.Create (canvas)) {
					canvas.BeginPaintRegion (new Gdk.Region ());
					context.SetSourceSurface (background, 0, 0);
					context.Paint ();

					foreach (Field field in player.Trail) {
						paintTrail (context, field.X * fieldSize, field.Y * fieldSize);
					}

					foreach (Ball ball in balls) {
						context.SetSourceRGB (1, 0, 0);		
						paintCircle (context, ball.X, ball.Y);
					}

					foreach (Monster monster in monsters) {
						paintMonster (context, monster);
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

		protected void rotateContext (Cairo.Context context, Direction direction)
		{
			switch (direction) {
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
		}

		protected void paintArrow (Cairo.Context context, Direction direction, int x, int y)
		{
			context.Save ();
			context.Translate (x, y);
			rotateContext (context, direction);
			context.MoveTo (fieldSize / 2, 0);
			context.LineTo (fieldSize, fieldSize);
			context.LineTo (fieldSize / 2, 3 * fieldSize / 4);
			context.LineTo (0, fieldSize);
			context.LineTo (fieldSize / 2, 0);
			context.FillPreserve ();
			context.NewPath ();
			context.Restore ();
		}

		protected void paintPlayer (Cairo.Context context, Player player)
		{
			context.SetSourceRGB (0, 0, 1);
			paintArrow (context, player.Direction, player.X, player.Y);
		}

		protected void paintMonster (Cairo.Context context, Monster monster)
		{
			switch (monster.Type) {
			case "Wanderer":
				context.SetSourceRGB (0, 1, 0);
				paintWanderer (context, monster);
				break;
			case "Sniffer":
				context.SetSourceRGB (1, 0, 1);
				paintSniffer (context, monster);
				break;
			case "Circulator":
				context.SetSourceRGB (0, 0, 0);
				paintCirculator (context, monster);
				break;
			default:
				context.SetSourceRGB (1, 1, 0);
				paintCircle (context, monster.X, monster.Y);
				break;
			}
		}

		protected void paintCirculator (Cairo.Context context, Monster monster)
		{
			paintArrow (context, monster.Direction, monster.X, monster.Y);
		}

		protected void paintSniffer (Cairo.Context context, Monster monster)
		{
			context.Save ();
			context.Translate (monster.X, monster.Y);
			rotateContext (context, monster.Direction);
			context.Arc (fieldSize / 2, fieldSize / 2, fieldSize / 3, 0, 2 * Math.PI);
			context.FillPreserve ();
			context.MoveTo (0, 0);
			context.LineTo (fieldSize, fieldSize);
			context.Stroke ();
			context.NewPath ();
			context.MoveTo (fieldSize, 0);
			context.LineTo (0, fieldSize);
			context.Stroke ();
			context.NewPath ();
			context.MoveTo (0, fieldSize / 2);
			context.LineTo (fieldSize, fieldSize / 2);
			context.Stroke ();
			context.NewPath ();
			context.Restore ();
		}

		protected void paintWanderer (Cairo.Context context, Monster monster)
		{
			context.Save ();
			context.Translate (monster.X + fieldSize / 2, monster.Y + fieldSize / 2);

			for (int i = 0; i < 12; i++) {
				context.LineTo ((fieldSize / 2) * Math.Cos (i * Math.PI / 6), (fieldSize / 2) * Math.Sin (i * Math.PI / 6));
				context.LineTo ((fieldSize / 3) * Math.Cos (Math.PI / 12 + i * Math.PI / 6), (fieldSize / 3) * Math.Sin (Math.PI / 12 + i * Math.PI / 6));
			}

			context.ClosePath ();
			context.FillPreserve ();
			context.NewPath ();

			context.Restore ();
		}
	}
}

