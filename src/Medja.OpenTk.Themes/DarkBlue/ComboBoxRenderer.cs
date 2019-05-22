using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class ComboBoxRenderer : SkiaControlRendererBase<ComboBox>
    {
        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _titlePaint;
        private readonly SKPaint _buttonPaint;
        private readonly SKPaint _dropDownArrowPaint;
        private readonly SKPaint _displayTextPaint;
        
        private readonly float _lineHeight;
        
        public ComboBoxRenderer(ComboBox control) : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.Color = control.Background.ToSKColor();
            _backgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(4,4,4,4, new SKColor(0,0,0,100), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground);

            _buttonPaint = new SKPaint();
            _buttonPaint.Color = DarkBlueThemeValues.PrimaryColor.ToSKColor();
            //_buttonPaint.ImageFilter = _backgroundPaint.ImageFilter;

            _dropDownArrowPaint = new SKPaint();
            _dropDownArrowPaint.Color = control.Background.ToSKColor();
            _dropDownArrowPaint.StrokeWidth = 1.5f;
            
            _titlePaint = new SKPaint();
            _titlePaint.Color = DarkBlueThemeValues.ControlBackground.ToSKColor();
            _titlePaint.Typeface = SKTypeface.FromFamilyName("Roboto", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);
            _titlePaint.TextSize = 16;
            _lineHeight = _titlePaint.TextSize * 1.3f;
            
            _displayTextPaint = new SKPaint();
            _displayTextPaint.Color = DarkBlueThemeValues.ControlBackground.ToSKColor();
            _displayTextPaint.Typeface = SKTypeface.FromFamilyName("Roboto");
            _displayTextPaint.TextSize = _titlePaint.TextSize;
        }

        protected override void InternalRender()
        {
            var buttonRect = new SKRect(_rect.Right - 35, _rect.Top, _rect.Right, _rect.Bottom);
            
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);

            if(_control.SelectedItem == null)
                _canvas.DrawTextSafe(_control.Title, _rect.Left + 10, _rect.Top + _lineHeight, _titlePaint);
            else
                _canvas.DrawTextSafe(_control.DisplayText, _rect.Left + 10, _rect.Top + _lineHeight, _displayTextPaint);
            
            _canvas.DrawRoundRect(buttonRect, 3, 3, _buttonPaint);
            
            // draw the v for drop down
            _canvas.DrawLine(_rect.Right - 25, _rect.Top + 11, _rect.Right - 17, _rect.Bottom - 10, _dropDownArrowPaint);
            _canvas.DrawLine(_rect.Right - 17, _rect.Bottom - 10, _rect.Right - 9, _rect.Top + 11, _dropDownArrowPaint);
        }
    }
}