using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.Demo
{
    public class ButtonRenderer : TextControlRendererBase<Button>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        private readonly SKPaint _disabledBackgroundPaint;
        private readonly SKPaint _clickBackgroundPaint;
        
        public ButtonRenderer(Button control) 
            : base(control)
        {
            _defaultBackgroundPaint = CreatePaint();
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            //_defaultBackgroundPaint.ImageFilter = DemoThemeValues.DropShadow;

            _disabledBackgroundPaint = CreatePaint();
            _disabledBackgroundPaint.Color = control.Background.GetDisabled().ToSKColor();
            _disabledBackgroundPaint.ImageFilter = DemoThemeValues.DropShadowDisabled;
            
            _clickBackgroundPaint = CreatePaint();
            _clickBackgroundPaint.Color = control.Background.ToSKColor();
            _clickBackgroundPaint.ImageFilter = DemoThemeValues.DropShadowElevated;
            ;
            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void DrawTextControlBackground()
        {
            var backgroundPaint = _defaultBackgroundPaint;

            if (!_control.IsEnabled)
                backgroundPaint = _disabledBackgroundPaint;
            else if (_control.InputState.IsLeftMouseDown)
                backgroundPaint = _clickBackgroundPaint;
            
            _canvas.DrawRoundRect(_rect, 3, 3, backgroundPaint);
        }
    }
}