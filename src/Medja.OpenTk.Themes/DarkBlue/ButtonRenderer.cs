using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ButtonRenderer : TextControlRendererBase<Button>
    {
        private readonly SKPaint _backgroundPaint;
        
        public ButtonRenderer(Button control)
            : base(control)
        {  
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            
            control.AffectRendering(control.PropertyBackground, 
                control.PropertyIsEnabled,
                control.InputState.PropertyIsMouseOver, 
                control.InputState.PropertyIsLeftMouseDown);
        }

        protected override void DrawTextControlBackground()
        {
            var rect = _control.Position.ToSKRect();
            
            if (_control.IsEnabled)
            {
                _backgroundPaint.Color = _control.Background.ToSKColor();

                if (_control.InputState.IsLeftMouseDown)
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowElevated;
                else
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            }
            else
            {
                _backgroundPaint.Color = _control.Background.GetDisabled().ToSKColor();
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