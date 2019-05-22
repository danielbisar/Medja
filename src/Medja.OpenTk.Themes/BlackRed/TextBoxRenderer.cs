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
		private readonly SKPaint _paint2;
		private readonly SKPaint _focusedPaint;
		private readonly SKPaint _disabledPaint;
		
		public TextBoxRenderer(TextBox textBox)
		: base(textBox)
		{
			_caretPaint = new SKPaint();
			_caretPaint.Color = BlackRedThemeValues.PrimaryTextColor.ToSKColor();
			_caretPaint.IsStroke = true;
			_caretPaint.IsAntialias = true;
			
			_caretStopWatch = Stopwatch.StartNew();
			
			_paint2 = new SKPaint();
			_paint2.IsAntialias = true;
			_paint2.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
            
			_focusedPaint = new SKPaint();
			_focusedPaint.IsAntialias = true;
			_focusedPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
			
			_disabledPaint = new SKPaint();
			_disabledPaint.IsAntialias = true;
			_disabledPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
		}

		protected override void DrawTextControlBackground()
		{
			RenderBottomLine();
		}

		private void RenderBottomLine()
		{
			SKPaint paint;

			if (_control.IsEnabled)
				paint = _control.IsFocused ? _paint2 : _focusedPaint;
			else
				paint = _disabledPaint;
			
			_canvas.DrawLine(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom, paint);
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

		protected override void Dispose(bool disposing)
		{
			_paint2.Dispose();
			_disabledPaint.Dispose();
			_focusedPaint.Dispose();
			_caretPaint.Dispose();
			
			base.Dispose(disposing);
		}
	}
}
