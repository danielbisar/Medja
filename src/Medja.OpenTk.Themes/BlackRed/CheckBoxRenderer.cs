using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class CheckBoxRenderer : TextControlRendererBase<CheckBox>
    {
        private readonly SKPaint _borderPaint;
        private readonly SKPaint _borderDisabledPaint;
        private readonly SKPaint _checkedPaint;
        private readonly SKPaint _checkedDisabledPaint;
        
        public CheckBoxRenderer(CheckBox checkBox)
            : base(checkBox)
        {
            _borderPaint = new SKPaint();
            _borderPaint.IsAntialias = true;
            _borderPaint.Color = BlackRedThemeValues.PrimaryLightColor.ToSKColor();
            _borderPaint.IsStroke = true;
            
            _borderDisabledPaint = new SKPaint();
            _borderDisabledPaint.IsAntialias = true;
            _borderDisabledPaint.Color = BlackRedThemeValues.PrimaryColor.ToSKColor();
            _borderDisabledPaint.IsStroke = true;
            
            _checkedPaint = new SKPaint();
            _checkedPaint.IsAntialias = true;
            _checkedPaint.Color = BlackRedThemeValues.SecondaryColor.ToSKColor();
            
            _checkedDisabledPaint = new SKPaint();
            _checkedDisabledPaint.IsAntialias = true;
            _checkedDisabledPaint.Color = BlackRedThemeValues.SecondaryLightColor.ToSKColor();
        }

        protected override void DrawTextControlBackground()
        {
            // todo set paints on change of IsEnabled to reduce CPU load
            var paint = _control.IsEnabled ? _borderPaint : _borderDisabledPaint;

            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _canvas.DrawRect(checkMarkBorder, paint);

            if (_control.IsChecked)
            {
                paint = _control.IsEnabled ? _checkedPaint : _checkedDisabledPaint;
                _canvas.DrawRect(checkMarkBorder, paint);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _borderPaint.Dispose();
                _borderDisabledPaint.Dispose();
                _checkedPaint.Dispose();
                _checkedDisabledPaint.Dispose();
            }
            
            base.Dispose(disposing);
        }
    }
}