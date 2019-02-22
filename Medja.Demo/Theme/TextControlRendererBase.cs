using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.Demo
{
    public abstract class TextControlRendererBase<T> : SkiaControlRendererBase<T> 
        where T : TextControl
    {
        protected readonly SKPaint _textPaint;
        protected readonly SKPaint _textDisabledPaint;
        
        private bool _isControlInitialized;
        protected float StartingY;
        
        public TextControlRendererBase(T control) 
            : base(control)
        {
            _textPaint = CreateTextPaint();
            _textDisabledPaint = CreateTextDisabledPaint();
        }

        protected abstract SKPaint CreateTextPaint();
        protected abstract SKPaint CreateTextDisabledPaint();

        protected override void InternalRender()
        {
            DrawTextControlBackground();
            
            // IsLayoutUpdated = true is necessary to be able to call GetLines
            if (!_control.IsLayoutUpdated)
                return;

            if (!_isControlInitialized)
                InitControl();
            
            DrawText();
        }
        
        protected abstract void DrawTextControlBackground();
        
        protected virtual void DrawText()
        {
            var paint = _control.IsEnabled ? _textPaint : _textDisabledPaint;
            var pos = _control.Position.ToSKPoint();
            // add the height also for the first line
            // else it seems the text is drawn at a 
            // too high position
            pos.Y += paint.TextSize + _control.Padding.Top;
            pos.X += _control.Padding.Left;

            StartingY = pos.Y;

            if (string.IsNullOrEmpty(_control.Text))
                return;
            
            var lines = _control.GetLines();
            var lineHeight = paint.TextSize * 1.3f;
			
            for (int i = 0; i < lines.Length && pos.Y <= _rect.Bottom; i++)
            {
                _canvas.DrawText(lines[i], pos, paint);
                pos.Y += lineHeight;
            }
        }
        
        private void InitControl()
        {
            var font = _control.Font;
			
            _textPaint.Typeface = font.Name == null ? DefaultTypeFace : SKTypeface.FromFamilyName(font.Name);
            _textPaint.TextSize = font.Size;
            _textPaint.Color = _control.TextColor.ToSKColor();

            _textDisabledPaint.Typeface = _textPaint.Typeface;
            _textDisabledPaint.TextSize = font.Size;
            _textDisabledPaint.Color = _control.TextColor.GetDisabled().ToSKColor();

            font.GetWidth = GetTextWidth;
			
            // TODO not supported scenarios:
            // - changing of foreground color of the control
            // - changing of any value in font of the control
            // - using the actual font and size defined in the font object

            _isControlInitialized = true;
        }

        private float GetTextWidth(string text)
        {
            return GetTextWidth(_textPaint, text);
        }
    }
}