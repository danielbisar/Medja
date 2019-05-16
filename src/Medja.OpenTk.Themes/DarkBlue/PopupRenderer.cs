using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class PopupRenderer : SkiaControlRendererBase<Popup>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        
        public PopupRenderer(Popup control) 
            : base(control)
        {
            _defaultBackgroundPaint = CreatePaint();
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            _defaultBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;

            control.PropertyBackground.PropertyChanged += OnBackgroundChanged;
        }

        // this should be the default behavior for all renderers, they should update the background if it changes
        private void OnBackgroundChanged(object sender, PropertyChangedEventArgs e)
        {
            _defaultBackgroundPaint.Color = _control.Background.ToSKColor();
        }

        protected override void InternalRender()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, _defaultBackgroundPaint);
        }
    }
}