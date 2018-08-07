using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// Provides extensions to support the conversion and other things between Medja and Skia
	/// </summary>
	public static class SkiaExtensions
	{
		public static SKRect ToSKRect(this MPosition position)
		{
			return new SKRect(position.X, position.Y, position.Width + position.X, position.Height + position.Y);
		}

		public static SKColor ToSKColor(this Color color)
		{
			if (color == null)
				return new SKColor(0, 0, 0, 0xFF);

			return new SKColor((byte)(0xFF * color.Red), (byte)(0xFF * color.Green), (byte)(0xFF * color.Blue));
		}

		/// <summary>
		/// Renders the specified text on the canvas with the given paint and centered inside the given rect.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="canvas">Canvas.</param>
		/// <param name="paint">Paint.</param>
		/// <param name="rect">Rect.</param>
		public static void RenderTextCentered(this SKCanvas canvas, string text, SKPaint paint, SKRect rect)
		{
			if (string.IsNullOrEmpty(text))
				return;

			var width = paint.MeasureText(text);
			var height = paint.TextSize;

			canvas.DrawText(text, rect.MidX - width / 2, rect.MidY + height / 2, paint);
		}

		/// <summary>
		/// Renders the specified text on the canvas with the given paint and inside the given rect.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="canvas">Canvas.</param>
		/// <param name="paint">Paint.</param>
		/// <param name="rect">Rect.</param>
		public static void RenderText(this SKCanvas canvas, string text, SKPaint paint, SKRect rect)
		{
			if (string.IsNullOrEmpty(text))
				return;

			canvas.DrawText(text, rect.Left + 5, rect.Top + paint.TextSize + 2, paint);
		}
	}
}
