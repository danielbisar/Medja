using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class MenuItemRenderer : SkiaControlRendererBase<MenuItem>
    {
        private readonly SKPaint _textPaint;
        private readonly SKPaint _backgroundPaint;

        public MenuItemRenderer(MenuItem control)
            : base(control)
        {
            control.AffectRendering(_control.PropertyBackground);
            
            _textPaint = new SKPaint();
            _textPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _textPaint.Typeface = SKTypeface.FromFamilyName("Roboto");
            _textPaint.TextSize = 16;
            _textPaint.IsAntialias = true;
            
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
        }

        protected override void InternalRender()
        {
            if (_control.Background != null)
            {
                _backgroundPaint.Color = _control.Background.ToSKColor();
                _canvas.DrawRect(_rect, _backgroundPaint);
            }

            _canvas.DrawTextSafe(_control.Title, _rect.Left + 10, _rect.Top + _textPaint.TextSize + 2, _textPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _textPaint.Dispose();
            _backgroundPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}