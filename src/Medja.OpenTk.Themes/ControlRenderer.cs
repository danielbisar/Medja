using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class ControlRenderer : SkiaControlRendererBase<Control>
    {
        private readonly SKPaint _backgroundPaint;
        
        public ControlRenderer(Control control) 
        : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
        }

        protected override void InternalRender()
        {
            if (_control.Background == null)
                return;
            
            var color = _control.IsEnabled ? _control.Background : _control.Background.GetDisabled();
            _backgroundPaint.Color = color.ToSKColor();
            
            _canvas.DrawRect(_rect, _backgroundPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}
