using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class CheckBoxRenderer : TextControlRendererBase<CheckBox>
    {
        private readonly SKPaint _borderPaint;
        
        public CheckBoxRenderer(CheckBox checkBox)
            : base(checkBox)
        {
            _borderPaint = new SKPaint();
            _borderPaint.IsAntialias = true;
            _borderPaint.StrokeWidth = 2;
            
            _control.AffectRendering(
                _control.PropertyBackground, 
                _control.PropertyIsChecked);
        }

        protected override void DrawTextControlBackground()
        {
            var rect = _control.Position.ToSKRect();
            var checkMarkBorder = new SKRect(rect.Left, rect.Top, rect.Left + rect.Height, rect.Bottom);

            _borderPaint.IsStroke = !_control.IsChecked;
            _borderPaint.Color = _control.Background.ToSKColor();
            _canvas.DrawRect(checkMarkBorder, _borderPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _borderPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}