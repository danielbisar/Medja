using Medja.Controls;
using Medja.Theming;
using SkiaSharp;
using Medja.Primitives;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// Skia control renderer base.
	/// 
	/// In Render call Render(canvas, control, ...) before using other render
	/// methods of this class.
	/// </summary>
	public abstract class SkiaControlRendererBase<TControl> : ControlRendererBase<SKCanvas, TControl>
		where TControl : Control
	{
		protected readonly SKPaint _paint;

		/// <summary>
		/// The canvas that is currently used.
		/// </summary>
		protected SKCanvas _canvas;

		/// <summary>
		/// The rect that is used for drawing the current control.
		/// </summary>
		protected SKRect _rect;

		protected TControl _control;

		public SkiaControlRendererBase()
		{
			_paint = new SKPaint();
			_paint.IsAntialias = true;
		}

		protected override void Render(SKCanvas context, TControl control)
		{
			try
			{
				_control = control;
				_canvas = context;
				_rect = control.Position.ToSKRect();

				InternalRender();
			}
			finally
			{
				_canvas = null;
				_control = null;
			}
		}

		protected abstract void InternalRender();

		protected void RenderBackground()
		{
			if (_control.Background == null)
				return;

			_paint.Color = _control.IsEnabled ?
				_control.Background.ToSKColor()
				: _control.Background.GetLighter(0.25f).ToSKColor();

			_canvas.DrawRect(_rect, _paint);
		}

		protected void RenderBorder(SKColor color, Thickness thickness)
		{
			_paint.Color = color;
			_paint.IsStroke = true;
			_paint.StrokeWidth = thickness.Left;

			// todo respect all thickness values left/right/top/bottom

			_canvas.DrawRect(_rect, _paint);
			_paint.IsStroke = false;
		}

		protected void RenderTextCentered(string text, Font font)
		{
			if (string.IsNullOrEmpty(text))
				return;

			BeginText(font);

			var width = _paint.MeasureText(text);
			var height = _paint.TextSize;

			_canvas.DrawText(text, _rect.MidX - width / 2, _rect.MidY + height / 2, _paint);

			EndText();
		}

		protected void RenderText(string text, Font font, SKPoint pos)
		{
			if (string.IsNullOrEmpty(text))
				return;

			BeginText(font);

			_canvas.DrawText(text, pos, _paint);

			EndText();
		}

		private void BeginText(Font font)
		{
			if (!string.IsNullOrEmpty(font.Name))
				_paint.Typeface = SKTypeface.FromFamilyName(font.Name, font.Style.ToSKTypefaceStyle());

			_paint.TextSize = font.Size;
		}

		private void EndText()
		{
			if (_paint.Typeface != null)
			{
				// TODO this is for now, we will maybe keep the typefaces in a 
				// cache depending on the performance
				_paint.Typeface.Dispose();
				_paint.Typeface = null;
			}
		}
	}
}
