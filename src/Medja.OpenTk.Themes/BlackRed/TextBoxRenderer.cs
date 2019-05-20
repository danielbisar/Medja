using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TextBoxRenderer : TextControlRendererBase<TextBox>
	{
		private readonly Stopwatch _caretStopWatch;
		private readonly SKPaint _caretPaint;

		public TextBoxRenderer(TextBox textBox)
		: base(textBox)
		{
			_caretPaint = CreatePaint();
			_caretPaint.Color = BlackRedThemeValues.PrimaryTextColor.ToSKColor();
			_caretPaint.IsStroke = true;
			
			_caretStopWatch = Stopwatch.StartNew();
		}

		protected override void DrawTextControlBackground()
		{
			RenderBottomLine();
		}

		private void RenderBottomLine()
		{
			if (!_control.IsEnabled)
				_paint.Color = BlackRedThemeValues.PrimaryLightColor.ToSKColor();
			else
			{
				if (_control.IsFocused)
				{
					_paint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
				}
				else
				{
					_paint.Color = BlackRedThemeValues.PrimaryLightColor.ToSKColor();
				}
			}

			_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, _paint);
		}
		
		protected override void DrawText()
		{
			var textWidth = 0f;
            
			if(!string.IsNullOrEmpty(_control.Text))
				textWidth = GetTextWidth(_textPaint, _control.Text.Substring(0, _control.CaretPos));
            
			var caretLeft = _rect.Left + _control.Padding.Left + textWidth;
			var maxX = _rect.Right - _control.Padding.Right;

			if (caretLeft > maxX)
			{
				var distance = caretLeft - maxX;
				XOffset = -distance;
			}
			else
				XOffset = 0;
            
			base.DrawText();
            
			if (_control.IsFocused && _caretStopWatch.ElapsedTicks % 10000000 <= 5000000)
			{
				var top = StartingY - _textPaint.TextSize;
				var bottom = top + _textPaint.FontSpacing;
                
				caretLeft = Math.Min(caretLeft, maxX);

				_canvas.DrawLine(new SKPoint(caretLeft, top), 
					new SKPoint(caretLeft, bottom), 
					_caretPaint);
			}
		}
	}
}
