using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly SKPaint _backgroundPaint;

		public TabControlRenderer(TabControl control)
			:base(control)
		{
			_backgroundPaint = new SKPaint();
			_backgroundPaint.IsAntialias = true;
		}
		
		protected override void InternalRender()
		{
			var rect = _control.Position.ToSKRect();

			if (_control.Background != null)
			{
				_backgroundPaint.Color = _control.Background.ToSKColor();
				_canvas.DrawRect(rect, _backgroundPaint);
			}
		}

		protected override void Dispose(bool disposing)
		{
			_backgroundPaint.Dispose();
			base.Dispose(disposing);
		}
	}
}
