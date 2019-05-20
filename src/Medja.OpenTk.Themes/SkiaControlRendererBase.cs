using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Themes
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
		/// todo this should not be here
		protected SKRect _rect;

		public SkiaControlRendererBase(TControl control)
			: base(control)
		{
			_paint = new SKPaint();
			_paint.IsAntialias = true;
		}

		protected SKPaint CreatePaint()
		{
			var result = new SKPaint();
			result.IsAntialias = true;

			return result;
		}

		protected override void Render(SKCanvas context)
		{
			try
			{
				_canvas = context;
				
				var hasClipping = !_control.ClippingArea.IsEmpty;

				if (hasClipping)
				{
					_canvas.Save();
					_canvas.ClipRect(_control.ClippingArea.ToSKRect());
				}

				_rect = _control.Position.ToSKRect();

				InternalRender();
				
				if(hasClipping)
					_canvas.Restore();
			}
			finally
			{
				_canvas = null;
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

		
		// todo this should not be here

		protected void RenderText(string text, Font font, SKPoint pos)
		{
			if (string.IsNullOrEmpty(text))
				return;

			_canvas.DrawText(text, pos, _paint);
		}

		// todo this should not be here
		protected float GetTextWidth(SKPaint paint, string text)
		{
			// paint.MeasureText throws an exception on text = null
			if (string.IsNullOrEmpty(text))
				return 0;

			return paint.MeasureText(text);
		}
	}
}
