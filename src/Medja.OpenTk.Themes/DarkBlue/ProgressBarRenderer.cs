using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ProgressBarRenderer : SkiaControlRendererBase<ProgressBar>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _filledPaint;
        
        public ProgressBarRenderer(ProgressBar control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            
            _filledPaint = new SKPaint();
            _filledPaint.IsAntialias = true;
            _filledPaint.ImageFilter = SKImageFilter.CreateErode(2,2);
            
            control.AffectRendering(control.PropertyValue, 
                control.PropertyMaxValue, 
                control.PropertyForeground,
                control.PropertyIsEnabled,
                control.PropertyBackground);
        }

        protected override void InternalRender()
        {
            if (_control.IsEnabled)
            {
                _backgroundPaint.Color = _control.Background.ToSKColor();
                _filledPaint.Color = _control.Foreground.ToSKColor();
            }
            else
            {
                _backgroundPaint.Color = _control.Background.GetDisabled().ToSKColor();
                _filledPaint.Color = _control.Foreground.GetDisabled().ToSKColor();
            }

            var rect = _control.Position.ToSKRect();
            var filledRect = new SKRect(rect.Left,
                rect.Top,
                rect.Left + rect.Width * _control.Percentage,
                rect.Bottom);
            
            _canvas.DrawRect(rect, _backgroundPaint);
            _canvas.DrawRect(filledRect, _filledPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            _filledPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}