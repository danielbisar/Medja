using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class VerticalScrollBarRenderer : SkiaControlRendererBase<VerticalScrollBar>
    {
        private readonly SKPaint _fillPaint;
        //private readonly BackgroundRenderer _backgroundRenderer;
        
        public VerticalScrollBarRenderer(VerticalScrollBar control)
        : base(control)
        {
            _fillPaint = new SKPaint();
            _fillPaint.IsAntialias = true;
            _fillPaint.Color = BlackRedThemeValues.PrimaryLightColor.ToSKColor();
            
          //  _backgroundRenderer = new BackgroundRenderer(control);
        }
        
        protected override void InternalRender()
        {
            //_backgroundRenderer.Render(_canvas);

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

        protected override void Dispose(bool disposing)
        {
            _fillPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}