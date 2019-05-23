using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TextBoxRenderer : TextControlRendererBase<TextBox>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _caretPaint;
        private readonly Stopwatch _caretStopWatch;
        
        public TextBoxRenderer(TextBox control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsStroke = true;
            _backgroundPaint.IsAntialias = true;

            _caretPaint = new SKPaint();
            _caretPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _caretPaint.IsStroke = true;
            
            _caretStopWatch = Stopwatch.StartNew();
            
            _control.AffectRendering(_control.PropertyIsEnabled, 
                _control.PropertyBackground, 
                _control.PropertyCaretPos,
                _control.PropertyIsFocused,
                _control.PropertyIsCaretVisible);
        }

        protected override void DrawTextControlBackground()
        {
            var color = DarkBlueThemeValues.ControlBorder;

            if (!_control.IsEnabled)
                color = color.GetDisabled();
            
            _backgroundPaint.Color = color.ToSKColor();
            
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
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