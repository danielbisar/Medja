using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class CheckBoxRenderer : SkiaControlRendererBase<CheckBox>
    {
        public CheckBoxRenderer(CheckBox checkBox)
            : base(checkBox)
        {
            
        }
        
        protected override void InternalRender()
        {
            RenderBackground();

            _paint.Color = _control.IsEnabled 
                    ? ColorMap.PrimaryLight.ToSKColor() 
                    : ColorMap.Primary.ToSKColor();

            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _paint.IsStroke = true; 
            _canvas.DrawRect(checkMarkBorder, _paint);
            _paint.IsStroke = false;

            if (_control.IsChecked)
            {
                _paint.Color = _control.IsEnabled 
                        ? ColorMap.Secondary.ToSKColor()
                        : ColorMap.SecondaryLight.ToSKColor();
                _canvas.DrawRect(checkMarkBorder, _paint);
            }

            _paint.Color = _control.IsEnabled ? ColorMap.PrimaryText.ToSKColor() : ColorMap.PrimaryLight.ToSKColor();
            RenderText(_control.Text, _control.Font, new SKPoint(checkMarkBorder.Right+5, checkMarkBorder.Top + _paint.FontSpacing));
        }
    }
}