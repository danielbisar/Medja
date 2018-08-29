using System;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TabControlRenderer : SkiaControlRendererBase<TabControl>
	{
		protected override void InternalRender()
		{
			var tabs = _control.Tabs;
			var padding = _control.Padding;
			var tabHeaderWidth = (_rect.Width - padding.LeftAndRight) / tabs.Count;
			var backgroundColor = _control.Background.ToSKColor();
			var headerRect = new SKRect(_rect.Left + padding.Left,
										_rect.Top + padding.Top,
										_rect.Left + padding.Left + tabHeaderWidth,
										_rect.Top + padding.Top + _control.HeaderHeight);

			foreach (var tab in tabs)
			{
				_paint.Color = tab.IsSelected ? ColorMap.PrimaryLight.ToSKColor() : ColorMap.Primary.ToSKColor();
				_canvas.DrawRect(headerRect, _paint);

				_paint.Color = ColorMap.Primary.ToSKColor();
				_canvas.DrawLine(headerRect.Right, headerRect.Top, headerRect.Right, headerRect.Bottom, _paint);

				_paint.Color = ColorMap.PrimaryText.ToSKColor();
				_canvas.RenderText(tab.Header, _paint, headerRect);

				headerRect.Left += tabHeaderWidth;
				headerRect.Right += tabHeaderWidth;
			}

			// content itself is rendered via rendering behavior of
			// content control
		}
	}
}
