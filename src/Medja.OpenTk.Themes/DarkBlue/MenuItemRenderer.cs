using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class MenuItemRenderer : SkiaControlRendererBase<MenuItem>
    {
        private static readonly SKPaint TextPaint;
        private static readonly SKPaint SelectedBackgroundPaint;
        
        static MenuItemRenderer()
        {
            TextPaint = new SKPaint();
            TextPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            TextPaint.Typeface = SKTypeface.FromFamilyName("Roboto");
            TextPaint.TextSize = 16;
            TextPaint.IsAntialias = true;
            
            SelectedBackgroundPaint = new SKPaint();
            SelectedBackgroundPaint.Color = DarkBlueThemeValues.PrimaryColor.ToSKColor();
            SelectedBackgroundPaint.IsAntialias = true;
        }

        public MenuItemRenderer(MenuItem control)
            : base(control)
        {
        }

        protected override void InternalRender()
        {
            if (_control.InputState.IsMouseOver)
                _canvas.DrawRect(_rect, SelectedBackgroundPaint);
            
            _canvas.DrawText(_control.Title, _rect.Left + 10, _rect.Top + TextPaint.TextSize + 2, TextPaint);
        }
    }
}