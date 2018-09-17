using Medja.Controls;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class CheckBoxRenderer : SkiaControlRendererBase<CheckBox>
    {
        protected override void InternalRender()
        {
            RenderBackground();

            _paint.Color = ColorMap.PrimaryLight.ToSKColor();

            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _paint.IsStroke = true; 
            _canvas.DrawRect(checkMarkBorder, _paint);
            _paint.IsStroke = false;

            if (_control.IsChecked)
            {
                _paint.Color = ColorMap.Secondary.ToSKColor();
                _canvas.DrawRect(checkMarkBorder, _paint);
            }

            _paint.Color = _control.IsEnabled ? ColorMap.PrimaryText.ToSKColor() : ColorMap.PrimaryLight.ToSKColor();
            RenderText(_control.Text, _control.Font, new SKPoint(checkMarkBorder.Right+5, checkMarkBorder.Top + _paint.FontSpacing));
        }
    }
}