using System;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TextBlockRenderer : SkiaControlRendererBase<TextBlock>
	{
		protected override void Render(SKCanvas context, TextBlock control)
		{
			if (string.IsNullOrWhiteSpace(control.Text))
				return;

			base.Render(context, control);
		}

		protected override void InternalRender()
		{
			RenderBackground();
			var pos = _control.Position.ToSKPoint();

			//if (control.TextWrapping == Primitives.TextWrapping.None)
			//{
			//  // shorten the text to renderable length
			//  while (paint.MeasureText(text) > rect.Width
			//         && text.Length > 1)
			//      text = text.Substring(0, text.Length - 2);
			//}

			_paint.Color = _control.IsEnabled ? SKColors.Black : SKColors.DarkGray;


			var lines = _control.Text.Split(new[] { "\n\r", "\n", "\r" }, StringSplitOptions.None);
			var lineHeight = _paint.FontSpacing;

			for (int i = 0; i < lines.Length; i++)
			{
				// add the height also for the first line
				// else it seems the text is drawn at a 
				// too high position
				pos.Y += lineHeight;
				_canvas.DrawText(lines[i], pos, _paint);
			}

			//paint.BreakText
		}
	}
}
