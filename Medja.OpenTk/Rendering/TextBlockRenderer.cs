using System;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TextBlockRenderer : SkiaControlRendererBase<TextBlock>
	{
		protected override void InternalRender()
		{
			RenderBackground();

			if (string.IsNullOrWhiteSpace(_control.Text))
				return;

			var pos = _control.Position.ToSKPoint();

			//if (control.TextWrapping == Primitives.TextWrapping.None)
			//{
			//  // shorten the text to renderable length
			//  while (paint.MeasureText(text) > rect.Width
			//         && text.Length > 1)
			//      text = text.Substring(0, text.Length - 2);
			//}

			_paint.Color = _control.IsEnabled
				? _control.Foreground.ToSKColor()
				: _control.Foreground.GetLighter(0.25f).ToSKColor();


			var lines = _control.Text.Split(new[] { "\n\r", "\n", "\r" }, StringSplitOptions.None);
			var lineHeight = _paint.FontSpacing;

			// add the height also for the first line
			// else it seems the text is drawn at a 
			// too high position
			pos.Y += lineHeight;

			for (int i = 0; i < lines.Length && pos.Y <= _rect.Bottom; i++)
			{
				RenderText(lines[i], _control.Font, pos);
				pos.Y += lineHeight;
			}
			//paint.BreakText
		}
	}
}
