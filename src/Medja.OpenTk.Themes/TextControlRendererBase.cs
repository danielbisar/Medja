using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public abstract class TextControlRendererBase<T> : SkiaControlRendererBase<T> 
        where T : TextControl
    {
        // TODO not supported scenarios:
        // - changing of any value in font of the control
        
        protected readonly SKPaint _textPaint;
        protected readonly SKPaint _textDisabledPaint;
        
        private SKPaint _currentTextPaint;
        
        private bool _isControlInitialized;
        protected float StartingY { get; private set; }
        protected float XOffset { get; set; }

        protected TextControlRendererBase(T control) 
            : base(control)
        {
            _textPaint = CreateTextPaint();
            _textDisabledPaint = CreateTextDisabledPaint();
            
            _control.Font.PropertyColor.PropertyChanged += OnTextColorChanged;
            
            _control.AffectRendering(_control.PropertyText);
        }

        private SKPaint CreateTextPaint()
        {
            // todo update on control.Font change
            var font = _control.Font;
            
            var result = new SKPaint();
            result.Typeface = font.Name == null ? null : SKTypeface.FromFamilyName(font.Name);
            result.TextSize = font.Size;
            result.Color = _control.Font.Color.ToSKColor();

            return result;
        }
        
        private SKPaint CreateTextDisabledPaint()
        {
            var result = new SKPaint();
            result.Typeface = _textPaint.Typeface;
            result.TextSize = _textPaint.TextSize;
            result.Color = _control.Font.Color.GetDisabled().ToSKColor();

            return result;
        }

        private void OnTextColorChanged(object sender, PropertyChangedEventArgs e)
        {
            _textPaint.Color = _control.Font.Color.ToSKColor();
            _textDisabledPaint.Color = _control.Font.Color.GetDisabled().ToSKColor();
        }

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
            _canvas.Save();
            _canvas.ClipRect(_control.TextClippingArea.ToSKRect());
            
            // todo set paints on change of IsEnabled to reduce CPU load
            _currentTextPaint = _control.IsEnabled ? _textPaint : _textDisabledPaint;
            var pos = _control.Position.ToSKPoint();
            // add the height also for the first line
            // else it seems the text is drawn at a 
            // too high position
            
            pos.Y += _currentTextPaint.TextSize + _control.Padding.Top;

            StartingY = pos.Y;

            if (string.IsNullOrEmpty(_control.Text))
            {
                _canvas.Restore();
                return;
            }
            // todo check if multiline is allowed
            
            var lines = _control.GetLines();
            var lineHeight = _currentTextPaint.TextSize * 1.3f;

            for (int i = 0; i < lines.Length && pos.Y <= _rect.Bottom; i++)
            {
                DrawTextLine(lines[i], pos);
                pos.Y += lineHeight;
            }
            
            _canvas.Restore();
        }

        private void DrawTextLine(string text, SKPoint pos)
        {
            switch (_control.TextAlignment)
            {
                case TextAlignment.Left:
                    _canvas.DrawText(text, pos.X + _control.Padding.Left + XOffset, pos.Y, _currentTextPaint);
                    break;
                case TextAlignment.Right:
                    _canvas.DrawText(text, _rect.Right - _control.Padding.Right - GetTextWidth(text) + XOffset, pos.Y, _currentTextPaint);
                    break;
                case TextAlignment.Center:
                    _canvas.DrawText(text, _rect.MidX - GetTextWidth(text) / 2 + XOffset, pos.Y, _currentTextPaint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitControl()
        {
            var font = _control.Font;
            font.GetWidth = GetTextWidth;

            _isControlInitialized = true;
        }

        private float GetTextWidth(string text)
        {
            return GetTextWidth(_currentTextPaint, text);
        }
        
        protected float GetTextWidth(SKPaint paint, string text)
        {
            // paint.MeasureText throws an exception on text = null
            if (string.IsNullOrEmpty(text))
                return 0;

            return paint.MeasureText(text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _textPaint.Dispose();
                _textDisabledPaint.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}