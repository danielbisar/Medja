using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class SliderRenderer : SkiaControlRendererBase<Slider>
    {
        private readonly SKPaint _barPaint;
        private readonly SKPaint _thumbPaint;
        
        public SliderRenderer(Slider control) 
            : base(control)
        {
            _barPaint = new SKPaint();
            _barPaint.StrokeCap = SKStrokeCap.Round;
            _barPaint.Style = SKPaintStyle.Stroke;
            _barPaint.StrokeWidth = 4;
            _barPaint.IsAntialias = true;
            
            _thumbPaint = new SKPaint();
            _thumbPaint.IsAntialias = true;
            
            control.AffectRendering(
                control.PropertyBackground, 
                control.PropertyThumbColor,
                control.PropertyPercentage);
        }

        protected override void InternalRender()
        {
            _barPaint.Color = _control.Background.ToSKColor();
            _thumbPaint.Color = _control.ThumbColor.ToSKColor();
            
            var y = _rect.MidY;
			
            _canvas.DrawLine(_rect.Left, y, _rect.Right, y, _barPaint);
            _canvas.DrawCircle(new SKPoint(_rect.Left + _rect.Width * _control.Percentage, _rect.MidY), 10, _thumbPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _barPaint.Dispose();
            _thumbPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}