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

        protected override SKPaint CreateTextPaint()
        {
            var result = CreatePaint();
            result.Color = DemoThemeValues.PrimaryTextColor.ToSKColor();

            return result;
        }

        protected override SKPaint CreateTextDisabledPaint()
        {
            var result = CreatePaint();
            result.Color = DemoThemeValues.PrimaryTextColor.GetDisabled().ToSKColor();

            return result;
        }

        protected override void DrawTextControlBackground()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, _textBoxBackground);
        }

        protected override void DrawText()
        {
            base.DrawText();
            
            if (_control.IsFocused && _caretStopWatch.ElapsedTicks % 10000000 <= 5000000)
            {
                var textWidth = GetTextWidth(_textPaint, _control.Text);
                var caretLeft = _rect.Left + _control.Padding.Left + textWidth;
                var top = StartingY - _textPaint.TextSize;
                var bottom = top + _textPaint.FontSpacing;

                _canvas.DrawLine(new SKPoint(caretLeft, top), 
                    new SKPoint(caretLeft, bottom), 
                    _caretPaint);
            }
        }
    }
}