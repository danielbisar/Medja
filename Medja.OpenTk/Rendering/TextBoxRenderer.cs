using System;
using Medja.Controls;
using SkiaSharp;
using System.Diagnostics;

namespace Medja.OpenTk.Rendering
{
	public class TextBoxRenderer : SkiaControlRendererBase<TextBox>
	{
		private readonly Stopwatch _caretStopWatch;

		public TextBoxRenderer()
		{
			_caretStopWatch = Stopwatch.StartNew();
		}

		protected override void InternalRender()
		{
			RenderBackground();
			RenderBorder(_control.Foreground.ToSKColor());

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

			pos.Y += _paint.FontSpacing;
			_canvas.DrawText(_control.Text, pos, _paint);

			if (_control.IsFocused && _caretStopWatch.ElapsedTicks % 10000000 <= 5000000)
			{
				var textLength = _paint.MeasureText(_control.Text);
				var caretLeft = _rect.Left + textLength;
				var top = _rect.Top + _paint.FontSpacing - _paint.TextSize;
				var bottom = top + _paint.FontSpacing;

				_canvas.DrawLine(new SKPoint(caretLeft, top), new SKPoint(caretLeft, bottom), _paint);
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
