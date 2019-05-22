using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ProgressBarRenderer : SkiaControlRendererBase<ProgressBar>
    {
        private readonly SKPaint _filledPaint;
        private readonly BackgroundRenderer _backgroundRenderer;
        
        public ProgressBarRenderer(ProgressBar control) 
            : base(control)
        {
            _backgroundRenderer = new BackgroundRenderer(control);
            
            _filledPaint = new SKPaint();
            _filledPaint.Color = _control.Foreground.ToSKColor();
            _filledPaint.ImageFilter = SKImageFilter.CreateErode(2,2);
            
            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void InternalRender()
        {
            _backgroundRenderer.Render(_canvas);
            
            var filledRect = new SKRect(_rect.Left,
                _rect.Top,
                _rect.Left + _rect.Width * _control.Percentage,
                _rect.Bottom);
            
            _canvas.DrawRect(filledRect, _filledPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundRenderer.Dispose();
            _filledPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}