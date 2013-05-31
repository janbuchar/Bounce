using System;
using System.Collections.Generic;
using Gtk;
using Mono;

namespace Bounce
{
	public class Board
	{
		protected Player player = new Player (0, 0);
		protected List<Ball> balls = new List<Ball> ();
		private Field[,] fields;
		const int fieldSize = 20;
		Cairo.Surface background;

		public Board (int width, int height, Gtk.DrawingArea area)
		{
			fields = new Field[width, height];
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					if (i == 0 || j == 0 || i + 1 == width || j + 1 == height) {
						fields [i, j].Full = true;
					} else {
						fields [i, j].Full = false;
					}
				}
			}
			area.SetSizeRequest (width * fieldSize, height * fieldSize);
			refreshBackground ();
		}

		protected void refreshBackground ()
		{
			background = new Cairo.ImageSurface (Cairo.Format.ARGB32, fields.GetLength (0) * fieldSize, fields.GetLength (1) * fieldSize);
			using (Cairo.Context context = new Cairo.Context(background)) {
				paintBackground (context);
			}
		}

		public void AddBall (Ball ball)
		{
			balls.Add (ball);
		}

		public void Fill (int x, int y)
		{
			fields [x, y].Full = true;
			refreshBackground ();
		}

		public void MoveBalls ()
		{
			foreach (Ball ball in balls) {
				for (int i = 0; i < 2; i++) {
					int posX = (ball.X + fieldSize / 2) / fieldSize;
					int posY = (ball.Y + ball.dY) / fieldSize;
					if (fields [posX, posY + 1].Full && ball.dY > 0) {
						ball.BounceY ();
					}
					if (fields [posX, posY].Full && ball.dY < 0) {
						ball.BounceY ();
					}
					posX = (ball.X + ball.dX) / fieldSize;
					posY = (ball.Y + fieldSize / 2) / fieldSize;
					if (fields [posX + 1, posY].Full && ball.dX > 0) {
						ball.BounceX ();
					}
					if (fields [posX, posY].Full && ball.dX < 0) {
						ball.BounceX ();
					}
					ball.X += ball.dX;
					ball.Y += ball.dY;	
				}
			}
			if (player.Moving) {
				player.Move (checkedPlayerDistance(5));
			}
			if (player.Remaining > 0) {
				player.Move (Math.Min (5, player.Remaining));
			}
		}

		public int checkedPlayerDistance (int steps)
		{
			int max = 0;
			switch (player.direction) {
			case Player.Direction.Down:
				max = (fields.GetLength (1)) * fieldSize - (player.Y + fieldSize);
				break;
			case Player.Direction.Up:
				max = player.Y;
				break;
			case Player.Direction.Right:
				max = (fields.GetLength (0)) * fieldSize - (player.X + fieldSize);
				break;
			case Player.Direction.Left:
				max = player.X;
				break;
			}
			return Math.Min (steps, max);
		}

		public void MovePlayer (Player.Direction direction)
		{
			if (!player.Moving && player.Remaining == 0) {
				player.StartMove (direction);
			}
		}

		public void StopPlayer ()
		{
			int steps = 0;
			switch (player.direction) {
			case Player.Direction.Up:
				steps = player.Y % fieldSize;
				break;
			case Player.Direction.Down:
				steps = fieldSize - player.Y % fieldSize;
				break;
			case Player.Direction.Right:
				steps = fieldSize - player.X % fieldSize;
				break;
			case Player.Direction.Left:
				steps = player.X % fieldSize;
				break;
			}
			player.Stop (checkedPlayerDistance(steps));
		}

		public void Render (Gdk.Window canvas)
		{
			using (Cairo.Context context = Gdk.CairoHelper.Create (canvas)) {
				canvas.BeginPaintRegion (new Gdk.Region());
				context.SetSourceSurface (background, 0, 0);
				context.Paint ();

				foreach (Ball ball in balls) {
					context.SetSourceRGB (1, 0, 0);		
					paintCircle (context, ball.X, ball.Y);
				}

				context.SetSourceRGB (0, 0, 1);
				paintCircle (context, player.X, player.Y);

				canvas.EndPaint ();
			}
		}

		protected void paintBackground (Cairo.Context context)
		{
			context.SetSourceRGB (0.8, 0.8, 0.8);
			context.Paint ();
			for (int i = 0; i < fields.GetLength(0); i++) {
				for (int j = 0; j < fields.GetLength(1); j++) {
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
			for (int i = 0; i < fields.GetLength(0); i++) {
				context.MoveTo (new Cairo.PointD (i * fieldSize, 0));
				context.LineTo (new Cairo.PointD (i * fieldSize, fields.GetLength (1) * fieldSize));
				context.Stroke ();
				context.NewPath ();
			}
			for (int i = 0; i < fields.GetLength(1); i++) {
				context.MoveTo (new Cairo.PointD (0, i * fieldSize));
				context.LineTo (new Cairo.PointD (fields.GetLength (0) * fieldSize, i * fieldSize));
				context.Stroke ();
				context.NewPath ();
			}
		}

		protected void paintSquare (Cairo.Context context, int x, int y, bool fill)
		{
			context.Save ();
			context.SetSourceRGB (0, 0, 1);
			context.Translate (x, y);
			context.Rectangle (new Cairo.Rectangle (new Cairo.Point (0, 0), fieldSize, fieldSize));
			context.SetSourceRGBA (0, 0, 0, fill ? 0.5 : 0.3);
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
	}
}

