using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class CheckBoxRenderer : TextControlRendererBase<CheckBox>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _checkMarkPaint;
        
        public CheckBoxRenderer(CheckBox control) 
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;

            _checkMarkPaint = new SKPaint();
            _checkMarkPaint.IsAntialias = true;
            _checkMarkPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _checkMarkPaint.IsStroke = true;
            _checkMarkPaint.StrokeWidth = 2;
            
            _control.AffectRendering(_control.PropertyBackground);
        }

        protected override void DrawTextControlBackground()
        {
            _backgroundPaint.Color = _control.Background.ToSKColor();
            
            if (_control.IsEnabled)
                _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            else
                _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;
            
            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _canvas.DrawRoundRect(checkMarkBorder, 2, 2, _backgroundPaint);

            if (_control.IsChecked)
            {
                using (var checkMarkPath = new SKPath())
                {
                    checkMarkPath.MoveTo(2 + _rect.Left, 10 + _rect.Top);
                    checkMarkPath.LineTo(7 + _rect.Left, 15 + _rect.Top);
                    checkMarkPath.LineTo(17 + _rect.Left, 5 + _rect.Top);

                    _canvas.DrawPath(checkMarkPath, _checkMarkPaint);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _checkMarkPaint.Dispose();
            _backgroundPaint.Dispose();
            
            base.Dispose(disposing);
        }
    }
}