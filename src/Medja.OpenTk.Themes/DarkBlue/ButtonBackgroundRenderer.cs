using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    /// <summary>
    /// Renders the background of a button.
    /// </summary>
    public class ButtonBackgroundRenderer : SkiaControlRendererBase<Control>
    {
        private readonly SKPaint _backgroundPaint;

        public ButtonBackgroundRenderer(Control control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;

            control.AffectRendering(control.PropertyBackground,
                control.PropertyIsEnabled,
                control.InputState.PropertyIsLeftMouseDown);
        }

        protected override void InternalRender()
        {
            var rect = _control.Position.ToSKRect();

            _backgroundPaint.Color = _control.Background.ToSKColor();

            if (_control.IsEnabled)
            {
                if (_control.InputState.IsLeftMouseDown)
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowElevated;
                else
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            }
            else
            {
                _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;
            }

            _canvas.DrawRoundRect(rect, 3, 3, _backgroundPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}