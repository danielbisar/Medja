using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class ScrollableContainerRenderer : SkiaControlRendererBase<ScrollableContainer>
    {
        private readonly SKPaint _scrollBackgroundPaint;
        
        public ScrollableContainerRenderer()
        {
            _scrollBackgroundPaint = new SKPaint();
            _scrollBackgroundPaint.IsAntialias = true;
            _scrollBackgroundPaint.Color = ColorMap.PrimaryLight.ToSKColor();
        }
        
        protected override void InternalRender()
        {
            if (_control.CanScroll)
            {
                var rect = new SKRect(_rect.Right - 20, _rect.Top, _rect.Right, _rect.Bottom);
                _canvas.DrawRect(rect, _scrollBackgroundPaint);
            }
        }
    }
}