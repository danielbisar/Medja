using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.BlackRed
{
    public class CheckBoxRenderer : TextControlRendererBase<CheckBox>
    {
        public CheckBoxRenderer(CheckBox checkBox)
            : base(checkBox)
        {
        }

        protected override void DrawTextControlBackground()
        {
            _paint.Color = _control.IsEnabled 
                ? BlackRedThemeValues.PrimaryLightColor.ToSKColor() 
                : BlackRedThemeValues.PrimaryColor.ToSKColor();

            var checkMarkBorder = new SKRect(_rect.Left, _rect.Top, _rect.Left + _rect.Height, _rect.Bottom);
            _paint.IsStroke = true; 
            _canvas.DrawRect(checkMarkBorder, _paint);
            _paint.IsStroke = false;

            if (_control.IsChecked)
            {
                _paint.Color = _control.IsEnabled 
                    ? BlackRedThemeValues.SecondaryColor.ToSKColor()
                    : BlackRedThemeValues.SecondaryLightColor.ToSKColor();
                _canvas.DrawRect(checkMarkBorder, _paint);
            }
        }
    }
}