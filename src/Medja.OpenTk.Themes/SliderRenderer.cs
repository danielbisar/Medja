using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class SliderRenderer : SkiaControlRendererBase<Slider>
    {
        private readonly SKPaint _barPaint;
        private readonly SKPaint _thumbPaint;
        private Action _render;
        
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
                control.PropertyPercentage,
                control.PropertyOrientation);
            
            control.PropertyOrientation.ForwardTo(p => UpdateRenderFunction());

            UpdateRenderFunction();
        }

        private void UpdateRenderFunction()
        {
            _render = _control.Orientation == Orientation.Horizontal 
                ? RenderHorizontal 
                : (Action)RenderVertical;
        }

        protected override void InternalRender()
        {
            _barPaint.Color = _control.Background.ToSKColor();
            _thumbPaint.Color = _control.ThumbColor.ToSKColor();
            
            _render();
        }

        private void RenderHorizontal()
        {
            var midY = _rect.MidY;

            _canvas.DrawLine(_rect.Left, midY, _rect.Right, midY, _barPaint);
            _canvas.DrawCircle(new SKPoint(_rect.Left + _rect.Width * _control.Percentage, midY), 10,
                _thumbPaint);
        }

        private void RenderVertical()
        {
            var midX = _rect.MidX;

            _canvas.DrawLine(midX, _rect.Top, midX, _rect.Bottom, _barPaint);
            _canvas.DrawCircle(new SKPoint(midX, _rect.Top + _rect.Height * _control.Percentage), 10,
                _thumbPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _barPaint.Dispose();
            _thumbPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}