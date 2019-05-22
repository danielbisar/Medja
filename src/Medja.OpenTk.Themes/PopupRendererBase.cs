using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Themes.DarkBlue;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public abstract class PopupRendererBase : SkiaControlRendererBase<Popup>
    {
        protected readonly SKPaint DefaultBackgroundPaint;
        
        protected PopupRendererBase(Popup control) 
            : base(control)
        {
            DefaultBackgroundPaint = new SKPaint();
            DefaultBackgroundPaint.Color = control.Background.ToSKColor();
            DefaultBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;

            control.PropertyBackground.PropertyChanged += OnBackgroundChanged;
        }

        private void OnBackgroundChanged(object sender, PropertyChangedEventArgs e)
        {
            DefaultBackgroundPaint.Color = _control.Background.ToSKColor();
        }
    }
}