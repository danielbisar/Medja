using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class PopupRenderer : SkiaControlRendererBase<Popup>
    {
        private readonly SKPaint _backgroundPaint;
        
        public PopupRenderer(Popup control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            
            _control.AffectRendering(_control.PropertyBackground);
        }
        
        protected override void InternalRender()
        {
            // do not render anything when background is null, else
            // transparency will not work always
            if (_control.Background == null)
                return;
            
            _backgroundPaint.Color = _control.Background.ToSKColor();
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}