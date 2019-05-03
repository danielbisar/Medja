using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TextEditorRenderer : SkiaControlRendererBase<TextEditor>
    {
        private SKPaint _textPaint;
        private SKPaint _selectionPaint;

        private readonly float _lineHeight;
        
        private SKPoint _selectionStart;
        private SKPoint _selectionEnd;
        
        public TextEditorRenderer(TextEditor control)
            : base(control)
        {
            _textPaint = CreatePaint();
            _textPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            _textPaint.TextSize = 16;

            _selectionPaint = CreatePaint();
            _selectionPaint.Color = DarkBlueThemeValues.Background.ToSKColor().WithAlpha(178);

            _lineHeight = _textPaint.TextSize * 1.3f;
        }

        protected override void InternalRender()
        {
            RenderBackground();
            
            var lines = _control.Lines;
            var x = _control.Position.X + 10;
            var y = _control.Position.Y + _lineHeight;
            var isInSelection = false;
            
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                
                // todo string marshalling is slow?
                _canvas.DrawText(line, x, y, _textPaint);
                
                if(i == _control.CaretY)
                    DrawCaret(x, y);

                var selectionStart = _control.GetLogicalSelectionStart();
                var selectionEnd = _control.GetLogicalSelectionEnd();
                
                var thisLineStartsSelection = i == selectionStart?.Y;
                var thisLineEndsSelection = i == selectionEnd?.Y;

                if (thisLineStartsSelection)
                    isInSelection = true;

                if (isInSelection)
                {
                    if (!thisLineStartsSelection && !thisLineEndsSelection)
                    {
                        // select whole line
                        _canvas.DrawRect(x, y - _textPaint.TextSize, GetTextWidth(line), _lineHeight, 
                            _selectionPaint);
                    }
                    else
                    {
                        string thisLinesSelectedText;
                        float selectionX = x;

                        if (thisLineStartsSelection)
                        {
                            var textBefore = line.Substring(0, selectionStart.X);
                            selectionX += GetTextWidth(textBefore);
                        }
                        
                        if (thisLineEndsSelection && thisLineStartsSelection)
                        {
                            thisLinesSelectedText = line.Substring(selectionStart.X,
                                selectionEnd.X - selectionStart.X);
                        }
                        else if (thisLineStartsSelection)
                        {
                            thisLinesSelectedText = line.Substring(selectionStart.X);
                        }
                        else //if (thisLineEndsSelection)
                            thisLinesSelectedText = line.Substring(0, selectionEnd.X);

                        var selectionWidth = GetTextWidth(thisLinesSelectedText);
                        
                        _canvas.DrawRect(selectionX, y-_textPaint.TextSize, selectionWidth, _textPaint.FontSpacing, _selectionPaint);
                    }
                }

                
                if (thisLineEndsSelection)
                    isInSelection = false;
                
                y += _lineHeight;
            }

        }

        private float GetTextWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return _textPaint.MeasureText(text);
        }

        private void DrawCaret(float startX, float y)
        {
            if (!_control.IsFocused 
                || Stopwatch.GetTimestamp() % 10000000 > 5000000) 
                return;
            
            // todo use intptr version and specify length, so we don't need to 
            // use substr
            var textBeforeCaret = _control.Lines[_control.CaretY].Substring(0, _control.CaretX) ?? "";
            float x;

            if (string.IsNullOrEmpty(textBeforeCaret))
                x = startX;
            else
                x = startX + _textPaint.MeasureText(textBeforeCaret);

            y -= _textPaint.TextSize;

            _canvas.DrawLine(x, y, x, y + _textPaint.FontSpacing, _textPaint);
        }
    }
}