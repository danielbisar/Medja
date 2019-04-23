using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly Font _font;
		
		public TabControlRenderer(TabControl control)
			:base(control)
		{
			_font = new Font();
		}
		
		protected override void InternalRender()
		{
			RenderBackground();
			
			var tabs = _control.Tabs;
			var tabHeaderWidth = _rect.Width / tabs.Count;
			var headerRect = new SKRect(_rect.Left,
										_rect.Top,
										_rect.Left + tabHeaderWidth,
										_rect.Top + _control.HeaderHeight);

			foreach (var tab in tabs)
			{
				_paint.Color = tab.IsSelected ? ColorMap.PrimaryLight.ToSKColor() : ColorMap.Primary.ToSKColor();
				_canvas.DrawRect(headerRect, _paint);

				_paint.Color = ColorMap.PrimaryText.ToSKColor();
				RenderText(tab.Header, _font, new SKPoint(headerRect.Left + 5, headerRect.Top + _paint.FontSpacing));

				headerRect.Left += tabHeaderWidth;
				headerRect.Right += tabHeaderWidth;
			}

			// content itself is rendered via rendering behavior of
			// content control
		}
	}
}
