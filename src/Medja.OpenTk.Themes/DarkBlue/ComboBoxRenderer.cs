using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ComboBoxRenderer : SkiaControlRendererBase<ComboBox2>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _titlePaint;
        private readonly SKPaint _buttonPaint;
        private readonly SKPaint _dropDownArrowPaint;
        
        private readonly float _lineHeight;
        
        public ComboBoxRenderer(ComboBox2 control) : base(control)
        {
            _backgroundPaint = CreatePaint();
            _backgroundPaint.Color = control.Background.ToSKColor();
            _backgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);

            _buttonPaint = CreatePaint();
            _buttonPaint.Color = DarkBlueThemeValues.PrimaryColor.ToSKColor();
            //_buttonPaint.ImageFilter = _backgroundPaint.ImageFilter;

            _dropDownArrowPaint = CreatePaint();
            _dropDownArrowPaint.Color = control.Background.ToSKColor();
            _dropDownArrowPaint.StrokeWidth = 1.5f;
            
            _titlePaint = CreatePaint();
            _titlePaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _titlePaint.Typeface = SKTypeface.FromFamilyName("Roboto", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);
            _titlePaint.TextSize = 16;
            _lineHeight = _titlePaint.TextSize * 1.3f;
        }

        protected override void InternalRender()
        {
            var buttonRect = new SKRect(_rect.Right - 35, _rect.Top, _rect.Right, _rect.Bottom);
            
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
            _canvas.DrawText(_control.Title, _rect.Left + 10, _rect.Top + _lineHeight, _titlePaint);
            _canvas.DrawRoundRect(buttonRect, 3, 3, _buttonPaint);
            
            // draw the v for drop down
            _canvas.DrawLine(_rect.Right - 25, _rect.Top + 11, _rect.Right - 17, _rect.Bottom - 10, _dropDownArrowPaint);
            _canvas.DrawLine(_rect.Right - 17, _rect.Bottom - 10, _rect.Right - 9, _rect.Top + 11, _dropDownArrowPaint);
        }
    }
}