using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly TextRenderer _textRenderer;
		private readonly SKPaint _headerBackgroundPaint;
		private readonly SKPaint _backgroundPaint;
		
		public TabControlRenderer(TabControl control)
			:base(control)
		{
			_textRenderer = new TextRenderer(new Font());
			
			_headerBackgroundPaint = new SKPaint();
			_headerBackgroundPaint.IsAntialias = true;
			
			_backgroundPaint = new SKPaint();
			_backgroundPaint.IsAntialias = true;
		}
		
		protected override void InternalRender()
		{
			var rect = _control.Position.ToSKRect();

			if (_control.Background != null)
			{
				var color = _control.IsEnabled ? _control.Background : _control.Background.GetDisabled();

				_backgroundPaint.IsStroke = false;
				_backgroundPaint.Color = color.ToSKColor();
				_canvas.DrawRect(rect, _backgroundPaint);
			}

			_backgroundPaint.Color = DarkBlueThemeValues.ControlBorder.ToSKColor();
			_backgroundPaint.IsStroke = true;
			_canvas.DrawRoundRect(rect, 3, 3, _backgroundPaint);

			//_canvas.Save();
			//_canvas.ClipRoundRect(new SKRoundRect(rect, 3, 3), SKClipOperation.Intersect, true);
		}

		protected override void Dispose(bool disposing)
		{
			_textRenderer.Dispose();
			_headerBackgroundPaint.Dispose();
			_backgroundPaint.Dispose();
			
			base.Dispose(disposing);
		}
	}
}
