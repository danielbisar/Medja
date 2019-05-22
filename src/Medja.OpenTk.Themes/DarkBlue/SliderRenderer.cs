using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class SliderRenderer : SkiaControlRendererBase<Slider>
    {
        private readonly SKPaint _barPaint;
        private readonly SKPaint _positionPaint;
        
        public SliderRenderer(Slider control) 
            : base(control)
        {
            _barPaint = new SKPaint();
            _barPaint.StrokeCap = SKStrokeCap.Round;
            _barPaint.Style = SKPaintStyle.Stroke;
            _barPaint.StrokeWidth = 4;
            _barPaint.IsAntialias = true;
            
            _positionPaint = new SKPaint();
            _positionPaint.IsAntialias = true;
            
            control.AffectRendering(control.PropertyBackground, 
                control.PropertyValue, 
                control.PropertyMaxValue, 
                control.PropertyMinValue, 
                control.PropertyIsEnabled);
        }

        protected override void InternalRender()
        {
            var color = _control.IsEnabled ? _control.Background : _control.Background.GetDisabled();
            
            _barPaint.Color = color.ToSKColor();

            color = _control.IsEnabled ? _control.Foreground : _control.Foreground.GetDisabled();

            _positionPaint.Color = color.ToSKColor();
            
            var y = _rect.MidY;
            var distance = _control.MaxValue - _control.MinValue;
            var value = _control.Value - _control.MinValue;
            var percentage = distance == 0 ? 0 : value / distance;
			
            _canvas.DrawLine(_rect.Left, y, _rect.Right, y, _barPaint);
            _canvas.DrawCircle(new SKPoint(_rect.Left + _rect.Width * percentage, _rect.MidY), 10, _positionPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _barPaint.Dispose();
            _positionPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}