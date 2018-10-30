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
            
            var filledRect = new SKRect(_rect.Left,
                                        scrollBarPos - 10,
                                        _rect.Right,
                                        scrollBarPos + 10);

            _canvas.DrawRect(filledRect, _fillPaint);
        }
    }
}