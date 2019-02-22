using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.Demo
{
    public class TextBoxRenderer : TextControlRendererBase<TextBox>
    {
        private readonly SKPaint _textBoxBackground;
        private readonly SKPaint _caretPaint;
        private readonly Stopwatch _caretStopWatch;
        
        public TextBoxRenderer(TextBox control) 
            : base(control)
        {
            _textBoxBackground = CreatePaint();
            _textBoxBackground.Color = DemoThemeValues.ControlBorder.ToSKColor();
            _textBoxBackground.IsStroke = true;

            _caretPaint = CreatePaint();
            _caretPaint.Color = DemoThemeValues.PrimaryTextColor.ToSKColor();
            _caretPaint.IsStroke = true;
            
            _caretStopWatch = Stopwatch.StartNew();
        }

        protected override void DrawTextControlBackground()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, _textBoxBackground);
        }

        protected override void DrawText()
        {
            var textWidth = GetTextWidth(_textPaint, _control.Text);
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