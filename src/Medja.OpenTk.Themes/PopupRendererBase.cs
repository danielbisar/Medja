using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Themes.DarkBlue;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public abstract class PopupRendererBase : SkiaControlRendererBase<Popup>
    {
        protected readonly SKPaint _backgroundPaint;
        
        protected PopupRendererBase(Popup control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.Color = control.Background.ToSKColor();
            _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            
            control.AffectRendering(control.PropertyBackground, control.PropertyIsEnabled);
        }
        
        protected override void InternalRender()
        {
            var color = _control.IsEnabled ? _control.Background : _control.Background.GetDisabled();
            _backgroundPaint.Color = color.ToSKColor();
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}