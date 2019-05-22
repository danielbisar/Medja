using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly TextRenderer _textRenderer;
		private readonly SKPaint _headerBackgroundPaint;
		private readonly SKPaint _selectedHeaderBackgroundPaint;
		private readonly BackgroundRenderer _backgroundRenderer;
		
		public TabControlRenderer(TabControl control)
			:base(control)
		{
			_textRenderer = new TextRenderer(new Font());
			_backgroundRenderer = new BackgroundRenderer(control);
			
			_headerBackgroundPaint = new SKPaint();
			_headerBackgroundPaint.IsAntialias = true;
			_headerBackgroundPaint.Color = BlackRedThemeValues.PrimaryColor.ToSKColor();
			
			_selectedHeaderBackgroundPaint = new SKPaint();
			_selectedHeaderBackgroundPaint.IsAntialias = true;
			_selectedHeaderBackgroundPaint.Color = BlackRedThemeValues.PrimaryLightColor.ToSKColor();
		}
		
		protected override void InternalRender()
		{
			_backgroundRenderer.Render(_canvas);
			
			var tabs = _control.Tabs;
			var tabHeaderWidth = _rect.Width / tabs.Count;
			var headerRect = new SKRect(_rect.Left,
										_rect.Top,
										_rect.Left + tabHeaderWidth,
										_rect.Top + _control.HeaderHeight);

			foreach (var tab in tabs)
			{
				var paint = tab.IsSelected ? _selectedHeaderBackgroundPaint : _headerBackgroundPaint;
				_canvas.DrawRect(headerRect, paint);

				_textRenderer.X = headerRect.Left + 5;
				_textRenderer.Y = headerRect.Top;
				_textRenderer.Render(tab.Header, _canvas);

				headerRect.Left += tabHeaderWidth;
				headerRect.Right += tabHeaderWidth;
			}

			// content itself is rendered via rendering behavior of
			// content control
		}

		protected override void Dispose(bool disposing)
		{
			_textRenderer.Dispose();
			_headerBackgroundPaint.Dispose();
			_selectedHeaderBackgroundPaint.Dispose();
			_backgroundRenderer.Dispose();
			
			base.Dispose(disposing);
		}
	}
}
