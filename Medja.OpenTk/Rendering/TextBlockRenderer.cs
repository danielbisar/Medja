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

			var font = _control.Font;

			if (!string.IsNullOrEmpty(font.Name))
				_paint.Typeface = SKTypeface.FromFamilyName(font.Name, font.Style.ToSKTypefaceStyle());

			_paint.TextSize = font.Size;


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
				_canvas.DrawText(lines[i], pos, _paint);
				pos.Y += lineHeight;
			}


			if (_paint.Typeface != null)
			{
				// TODO this is for now, we will maybe keep the typefaces in a 
				// cache depending on the performance
				_paint.Typeface.Dispose();
				_paint.Typeface = null;
			}

			//paint.BreakText
		}
	}
}
