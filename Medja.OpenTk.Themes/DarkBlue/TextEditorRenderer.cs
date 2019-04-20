using System;
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
                y += _lineHeight;
            }
        }
    }
}