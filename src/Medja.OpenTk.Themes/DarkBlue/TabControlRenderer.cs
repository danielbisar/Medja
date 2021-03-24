using Medja.Controls.Container;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly SKPaint _backgroundPaint;
		private readonly SKPaint _borderPaint;

		public TabControlRenderer(TabControl control)
			:base(control)
		{
			_backgroundPaint = new SKPaint();
			_backgroundPaint.IsAntialias = true;

			_borderPaint = new SKPaint();
			_borderPaint.IsAntialias = true;
			_borderPaint.IsStroke = true;
			_borderPaint.Color = ThemeDarkBlueValues.ControlBorder.ToSKColor();
		}

		protected override void InternalRender()
		{
			var rect = _control.Position.ToSKRect();

			if (_control.Background != null)
			{
				_backgroundPaint.Color = _control.Background.ToSKColor();
				_canvas.DrawRect(rect, _backgroundPaint);
			}

			_canvas.DrawRoundRect(rect, 3, 3, _borderPaint);
		}

		protected override void Dispose(bool disposing)
		{
			_backgroundPaint.Dispose();
			_borderPaint.Dispose();

			base.Dispose(disposing);
		}
	}
}
