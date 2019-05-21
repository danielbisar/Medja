using System;
using System.Diagnostics;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    public class TextEditorRenderer : SkiaControlRendererBase<TextEditor>
    {
        private static readonly SKPaint TextPaint;
        private static readonly SKPaint SelectionPaint;

        static TextEditorRenderer()
        {
            TextPaint = new SKPaint();
            TextPaint.IsAntialias = true;
            TextPaint.Color = DarkBlueThemeValues.PrimaryTextColor.ToSKColor();
            TextPaint.TextSize = 16;

            SelectionPaint = new SKPaint();
            SelectionPaint.IsAntialias = true;
            SelectionPaint.Color = DarkBlueThemeValues.Background.ToSKColor().WithAlpha(178);
        }

        private readonly float _lineHeight;
        private bool _isInSelection;

        public TextEditorRenderer(TextEditor control)
            : base(control)
        {
            _lineHeight = TextPaint.TextSize * 1.3f;
        }

        protected override void InternalRender()
        {
            RenderBackground();

            var lines = _control.Lines;
            var x = _control.Position.X + 10;
            var y = _control.Position.Y + _lineHeight;

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                // todo string marshalling is slow?
                _canvas.DrawTextSafe(line, x, y, TextPaint);

                if (i == _control.CaretY)
                    DrawCaret(x, y);

                DrawSelection(x, y, i);

                y += _lineHeight;
            }
        }

        private void DrawSelection(float x, float y, int lineIndex)
        {
            var line = _control.Lines[lineIndex];
            var selectionStart = _control.GetLogicalSelectionStart();
            var selectionEnd = _control.GetLogicalSelectionEnd();

            var thisLineStartsSelection = lineIndex == selectionStart?.Y;
            var thisLineEndsSelection = lineIndex == selectionEnd?.Y;

            if (thisLineStartsSelection)
                _isInSelection = true;

            if (_isInSelection)
            {
                if (!thisLineStartsSelection && !thisLineEndsSelection)
                {
                    // select whole line
                    _canvas.DrawRect(x, y - TextPaint.TextSize, GetTextWidth(line), _lineHeight,
                        SelectionPaint);
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

                    _canvas.DrawRect(selectionX, y - TextPaint.TextSize, selectionWidth, TextPaint.FontSpacing,
                        SelectionPaint);
                }
            }


            if (thisLineEndsSelection)
                _isInSelection = false;
        }

        private float GetTextWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return TextPaint.MeasureText(text);
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
                x = startX + TextPaint.MeasureText(textBeforeCaret);

            y -= TextPaint.TextSize;

            _canvas.DrawLine(x, y, x, y + TextPaint.FontSpacing, TextPaint);
        }
    }
}