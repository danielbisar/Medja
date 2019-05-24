using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TextBoxRenderer : TextControlRendererBase<TextBox>
	{
		private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _caretPaint;
        
        public TextBoxRenderer(TextBox control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsStroke = true;
            _backgroundPaint.IsAntialias = true;

            _caretPaint = new SKPaint();
            _caretPaint.Color = BlackRedThemeValues.PrimaryTextColor.ToSKColor();
            _caretPaint.IsStroke = true;
            
            _control.AffectRendering(_control.PropertyBackground, 
                _control.PropertyCaretPos,
                _control.PropertyIsFocused,
                _control.PropertyIsCaretVisible);
        }

        protected override void DrawTextControlBackground()
        {
            var rect = _control.Position.ToSKRect();
            _backgroundPaint.Color = _control.Background.ToSKColor();
            _canvas.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, _backgroundPaint);
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
            
            if (_control.IsCaretVisible)
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
            _backgroundPaint.Dispose();
            _caretPaint.Dispose();
            
            base.Dispose(disposing);
        }
	}
}
