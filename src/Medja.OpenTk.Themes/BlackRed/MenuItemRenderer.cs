using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class MenuItemRenderer : SkiaControlRendererBase<MenuItem>
    {
        private static readonly SKPaint TextPaint;
        private static readonly SKPaint SelectedBackgroundPaint;
        
        static MenuItemRenderer()
        {
            TextPaint = new SKPaint();
            TextPaint.Color = BlackRedThemeValues.PrimaryTextColor.ToSKColor();
            TextPaint.Typeface = SKTypeface.FromFamilyName("Monospace");
            TextPaint.TextSize = 16;
            TextPaint.IsAntialias = true;
            
            SelectedBackgroundPaint = new SKPaint();
            SelectedBackgroundPaint.Color = BlackRedThemeValues.PrimaryColor.ToSKColor();
            SelectedBackgroundPaint.IsAntialias = true;
        }

        private readonly SKPaint _backgroundPaint;

        public MenuItemRenderer(MenuItem control)
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            _backgroundPaint.Color = control.Background.ToSKColor();
            control.PropertyBackground.PropertyChanged += OnControlBackgroundChanged;
        }

        private void OnControlBackgroundChanged(object sender, PropertyChangedEventArgs e)
        {
            _backgroundPaint.Color = _control.Background.ToSKColor();
        }

        protected override void InternalRender()
        {
            if (_control.IsSelected)
                _canvas.DrawRect(_rect, SelectedBackgroundPaint);
            else if(_control.Background != null)
                _canvas.DrawRect(_rect, _backgroundPaint);
            
            _canvas.DrawText(_control.Title, _rect.Left + 10, _rect.Top + TextPaint.TextSize + 2, TextPaint);
        }
    }
}