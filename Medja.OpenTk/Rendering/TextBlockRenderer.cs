using System;
using Medja.Controls;
using Medja.Theming;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	public class TextBlockRenderer : ControlRendererBase<SKCanvas, TextBlock>
	{
		protected override void Render(SKCanvas canvas, TextBlock control)
		{
			var text = control.Text;

			if (string.IsNullOrWhiteSpace(text))
				return;

			//var rect = control.Position.ToSKRect();
			var pos = control.Position.ToSKPoint();

			using (var paint = new SKPaint())
			{
				paint.IsAntialias = true;
				paint.Color = control.IsEnabled ? SKColors.Black : SKColors.DarkGray;

				//if (control.TextWrapping == Primitives.TextWrapping.None)
				//{
				//	// shorten the text to renderable length
				//	while (paint.MeasureText(text) > rect.Width
				//		   && text.Length > 1)
				//		text = text.Substring(0, text.Length - 2);
				//}

				var lines = text.Split(new[] { "\n\r", "\n", "\r" }, StringSplitOptions.None);
				var lineHeight = paint.FontSpacing;

				for (int i = 0; i < lines.Length; i++)
				{
					// add the height also for the first line
					// else it seems the text is drawn at a 
					// too high position
					pos.Y += lineHeight;
					canvas.DrawText(lines[i], pos, paint);
				}

				//paint.BreakText
			}
		}
	}
}
