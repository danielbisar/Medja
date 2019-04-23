using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Themes.BlackRed;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ProgressBarRenderer : SkiaControlRendererBase<ProgressBar>
    {
        private readonly SKPaint _filledPaint;
        
        public ProgressBarRenderer(ProgressBar control) 
            : base(control)
        {
            _filledPaint = CreatePaint();
            _filledPaint.Color = _control.Foreground.ToSKColor();
            _filledPaint.ImageFilter = SKImageFilter.CreateErode(2,2);
            
            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void InternalRender()
        {
            RenderBackground();

            var filledRect = new SKRect(_rect.Left,
                _rect.Top,
                _rect.Left + _rect.Width * _control.Percentage,
                _rect.Bottom);
            
            _canvas.DrawRect(filledRect, _filledPaint);
        }
    }
}