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
        
        private bool _isControlInitialized;
        protected float StartingY { get; private set; }
        protected float XOffset { get; set; }

        protected TextControlRendererBase(T control) 
            : base(control)
        {
            var font = _control.Font;
            _textPaint = new SKPaint();
            _textPaint.Typeface = font.Name == null ? null : SKTypeface.FromFamilyName(font.Name);
            _textPaint.TextSize = font.Size;
            _textPaint.IsAntialias = true;
            
            _control.AffectRendering(_control.PropertyText, _control.Font.PropertyColor);
            
            // todo update textpaint on font changed; do not do this in InternalRender because SKTypeface lookup is
            // expensive
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
            
            _textPaint.Color = _control.Font.Color.ToSKColor();
           
            // todo set paints on change of IsEnabled to reduce CPU load
            var pos = _control.Position.ToSKPoint();
            // add the height also for the first line
            // else it seems the text is drawn at a 
            // too high position
            
            pos.Y += _textPaint.TextSize + _control.Padding.Top;

            StartingY = pos.Y;

            if (string.IsNullOrEmpty(_control.Text))
            {
                _canvas.Restore();
                return;
            }
            // todo check if multiline is allowed
            
            var lines = _control.GetLines();
            var lineHeight = _textPaint.TextSize * 1.3f;

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
                    _canvas.DrawText(text, pos.X + _control.Padding.Left + XOffset, pos.Y, _textPaint);
                    break;
                case TextAlignment.Right:
                    _canvas.DrawText(text, _rect.Right - _control.Padding.Right - GetTextWidth(text) + XOffset, pos.Y, _textPaint);
                    break;
                case TextAlignment.Center:
                    _canvas.DrawText(text, _rect.MidX - GetTextWidth(text) / 2 + XOffset, pos.Y, _textPaint);
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
            return GetTextWidth(_textPaint, text);
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
            _textPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}