using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.Demo
{
    public class CheckBoxRenderer : SkiaControlRendererBase<CheckBox>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        private readonly SKPaint _disabledUncheckdBackgroundPaint;
        private readonly SKPaint _checkedBackgroundPaint;
        private readonly SKPaint _disabledCheckedBackgroundPaint;
        private readonly SKPaint _checkMarkPaint;
        
        public CheckBoxRenderer(CheckBox control) 
            : base(control)
        {
            _defaultBackgroundPaint = CreatePaint();
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            _defaultBackgroundPaint.ImageFilter = DemoThemeValues.DropShadow;
            
            _disabledUncheckdBackgroundPaint = CreatePaint();
            _disabledUncheckdBackgroundPaint.Color = control.Background.GetDisabled().ToSKColor();
            _disabledUncheckdBackgroundPaint.ImageFilter = DemoThemeValues.DropShadowDisabled;

            _checkedBackgroundPaint = CreatePaint();
            _checkedBackgroundPaint.Color = DemoThemeValues.PrimaryColor.ToSKColor(); // todo control.F.ToSKColor();
            _checkedBackgroundPaint.ImageFilter = DemoThemeValues.DropShadow;
            
            _disabledCheckedBackgroundPaint = CreatePaint();
            _disabledCheckedBackgroundPaint.Color = DemoThemeValues.PrimaryColor.ToSKColor(); // todo control.F.ToSKColor();
            _disabledCheckedBackgroundPaint.ImageFilter = DemoThemeValues.DropShadowDisabled;

            _checkMarkPaint = CreatePaint();
            _checkMarkPaint.Color = DemoThemeValues.PrimaryTextColor.ToSKColor();
            _checkMarkPaint.IsStroke = true;
            _checkMarkPaint.StrokeWidth = 2;

            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void InternalRender()
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
                    : _disabledUncheckdBackgroundPaint;
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

            // todo implement a better way to draw text; it should include caching of the font, color and size
            // maybe a simple class for rendering text?
            _paint.Color = _control.IsEnabled ? _control.TextColor.ToSKColor() : _control.TextColor.GetDisabled().ToSKColor();
            RenderText(_control.Text, _control.Font, new SKPoint(checkMarkBorder.Right+5, checkMarkBorder.MidY + _paint.TextSize / 2.2f));
        }
    }
}