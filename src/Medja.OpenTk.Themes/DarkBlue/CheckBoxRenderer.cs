using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class CheckBoxRenderer : TextControlRendererBase<CheckBox>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        private readonly SKPaint _disabledUncheckedBackgroundPaint;
        private readonly SKPaint _checkedBackgroundPaint;
        private readonly SKPaint _disabledCheckedBackgroundPaint;
        private readonly SKPaint _checkMarkPaint;
        
        public CheckBoxRenderer(CheckBox control) 
            : base(control)
        {
            _defaultBackgroundPaint = new SKPaint();
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            _defaultBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            _defaultBackgroundPaint.IsAntialias = true;
            
            _disabledUncheckedBackgroundPaint = new SKPaint();
            _disabledUncheckedBackgroundPaint.IsAntialias = true;
            _disabledUncheckedBackgroundPaint.Color = control.Background.GetDisabled().ToSKColor();
            _disabledUncheckedBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;

            _checkedBackgroundPaint = new SKPaint();
            _checkedBackgroundPaint.IsAntialias = true;
            _checkedBackgroundPaint.Color = DarkBlueThemeValues.PrimaryColor.ToSKColor(); // todo control.F.ToSKColor();
            _checkedBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            
            _disabledCheckedBackgroundPaint = new SKPaint();
            _disabledCheckedBackgroundPaint.IsAntialias = true;
            _disabledCheckedBackgroundPaint.Color = DarkBlueThemeValues.PrimaryColor.ToSKColor(); // todo control.F.ToSKColor();
            _disabledCheckedBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;

            _checkMarkPaint = new SKPaint();
            _checkMarkPaint.IsAntialias = true;
            _checkMarkPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _checkMarkPaint.IsStroke = true;
            _checkMarkPaint.StrokeWidth = 2;

            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void DrawTextControlBackground()
        {
            SKPaint backgroundPaint = null;
            
            // todo do not get the actual state on rendering but in ctor and watch for changes of control
            // we might need to implement dispose...

            if (_control.IsEnabled)
            {
                backgroundPaint = _control.IsChecked ? _checkedBackgroundPaint : _defaultBackgroundPaint;
            }
            else
            {
                backgroundPaint = _control.IsChecked
                    ? _disabledCheckedBackgroundPaint
                    : _disabledUncheckedBackgroundPaint;
            }
            
            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _canvas.DrawRoundRect(checkMarkBorder, 2, 2, backgroundPaint);

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
    }
}