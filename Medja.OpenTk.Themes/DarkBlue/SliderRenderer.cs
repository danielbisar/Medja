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
            _barPaint = CreatePaint();
            _barPaint.Color = _control.Background.ToSKColor();
            _barPaint.StrokeCap = SKStrokeCap.Round;
            _barPaint.Style = SKPaintStyle.Stroke;
            _barPaint.StrokeWidth = 4;

            _positionPaint = CreatePaint();
            _positionPaint.Color = _control.Foreground.ToSKColor();
        }

        protected override void InternalRender()
        {
            var y = _rect.MidY;
            var distance = _control.MaxValue - _control.MinValue;
            var value = _control.Value - _control.MinValue;
            var percentage = distance == 0 ? 0 : value / distance;
			
            _canvas.DrawLine(_rect.Left, y, _rect.Right, y, _barPaint);
            _canvas.DrawCircle(new SKPoint(_rect.Left + _rect.Width * percentage, _rect.MidY), 10, _positionPaint);
        }
    }
}