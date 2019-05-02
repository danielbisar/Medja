using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TextEditorRenderer : SkiaControlRendererBase<TextEditor>
    {
        private SKPaint _textPaint;
        private readonly float _lineHeight;
        
        public TextEditorRenderer(TextEditor control)
            : base(control)
        {
            _textPaint = CreatePaint();
            _textPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            //_textPaint.
            _textPaint.TextSize = 16;

            _lineHeight = _textPaint.TextSize * 1.3f;
        }

        protected override void InternalRender()
        {
            RenderBackground();
            
            var lines = _control.Lines;
            var x = _control.Position.X + 10;
            var y = _control.Position.Y + _lineHeight;
            
            for (int i = 0; i < lines.Count; i++)
            {
                // todo string marshalling is slow?
                _canvas.DrawText(lines[i], x, y, _textPaint);
                
                if(i == _control.CaretY)
                    DrawCaret(x, y);
                    
                y += _lineHeight;
            }

        }

        private void DrawCaret(float startX, float y)
        {
            if (!_control.IsFocused 
                || Stopwatch.GetTimestamp() % 10000000 > 5000000) 
                return;
            
            // todo use intptr version and specify length, so we don't need to 
            // use substr
            var textBeforeCaret = _control.Lines[_control.CaretY].Substring(0, _control.CaretX) ?? "";
            float x;

            if (string.IsNullOrEmpty(textBeforeCaret))
                x = startX;
            else
                x = startX + _textPaint.MeasureText(textBeforeCaret);

            y -= _textPaint.TextSize;

            _canvas.DrawLine(x, y, x, y + _textPaint.FontSpacing, _textPaint);
        }
    }
}