using System.Collections.Generic;
using Medja.Controls;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		private readonly Font _font;
		
		public TabControlRenderer()
		{
			_font = new Font();
		}
		
		protected override void InternalRender()
		{
			var tabs = _control.Tabs;
			var padding = _control.Padding;
			var tabHeaderWidth = (_rect.Width - padding.LeftAndRight) / tabs.Count;
			var headerRect = new SKRect(_rect.Left + padding.Left,
										_rect.Top + padding.Top,
										_rect.Left + padding.Left + tabHeaderWidth,
										_rect.Top + padding.Top + _control.HeaderHeight);

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
