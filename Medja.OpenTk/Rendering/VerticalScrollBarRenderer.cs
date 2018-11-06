using System;
using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class VerticalScrollBarRenderer : SkiaControlRendererBase<VerticalScrollBar>
    {
        private readonly SKPaint _fillPaint;
        
        public VerticalScrollBarRenderer()
        {
            _fillPaint = new SKPaint();
            _fillPaint.IsAntialias = true;
            _fillPaint.Color = ColorMap.PrimaryLight.ToSKColor();
        }
        
        protected override void InternalRender()
        {
            RenderBackground();

            var scrollBarPos = _rect.Top + _rect.Height * _control.Percentage;
            var top = scrollBarPos - 10;

            if (top < _rect.Top)
                top = _rect.Top;

            var bottom = scrollBarPos + 10;

            if (bottom > _rect.Bottom)
                bottom = _rect.Bottom;
            
            var filledRect = new SKRect(_rect.Left, 
                                        top,
                                        _rect.Right,
                                        bottom);

            _canvas.DrawRect(filledRect, _fillPaint);
        }
    }
}