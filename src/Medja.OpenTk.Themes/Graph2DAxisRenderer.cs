using Medja.Controls.Graph2D;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class Graph2DAxisRenderer : SkiaControlRendererBase<Graph2DAxis>
    {
        // use this because the one of the base class will be replaced soon
        private new SKRect _rect;

        private readonly SKPaint _backgroundPaint;
        private readonly SKPaint _textPaint;

        public Graph2DAxisRenderer(Graph2DAxis control)
            : base(control)
        {
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            _backgroundPaint.IsStroke = true;

            _textPaint = new SKPaint();
            _textPaint.IsAntialias = true;

            // todo should be updated on change; since this is expensive we don't do it in render
            _textPaint.Typeface = SKTypeface.FromFamilyName(_control.Font.Name);

            _control.AffectRendering(
                _control.PropertyBackground,
                _control.PropertyOrientation,
                _control.Font.PropertyColor,
                _control.Font.PropertySize);
        }

        protected override void InternalRender()
        {
            _rect = _control.Position.ToSKRect();

            _backgroundPaint.Color = _control.Background.ToSKColor();

            _textPaint.Color = _control.Font.Color.ToSKColor();
            _textPaint.TextSize = _control.Font.Size;

            if (_control.Orientation == AxisOrientation.Horizontal)
                DrawHorizontal();
            else
                DrawVertical();
        }

        private void DrawHorizontal()
        {
            _canvas.DrawLine(_rect.Left, _rect.Top, _rect.Right, _rect.Top, _backgroundPaint);
        }

        private void DrawVertical()
        {
            _rect.Top += _textPaint.TextSize;

            _canvas.DrawLine(_rect.Right, _rect.Top, _rect.Right, _rect.Bottom, _backgroundPaint);

            // todo create function that is able to draw the mark and text easily and helps with the positioning
            // todo after that draw multiple ticks

            // max value tick and text
            var maxText = _control.Max.ToString();
            var textWidth = _textPaint.GetTextWidth(maxText);

            _canvas.DrawLine(_rect.Right - 10, _rect.Top, _rect.Right, _rect.Top, _backgroundPaint);
            _canvas.DrawTextSafe(maxText, _rect.Right - 20 - textWidth, _rect.Top + 1 + _textPaint.FontMetrics.Bottom / 2.0f, _textPaint);

            // min value tick and text
            var minText = _control.Min.ToString();
            textWidth = _textPaint.GetTextWidth(minText);

            _canvas.DrawLine(_rect.Right - 10, _rect.Bottom, _rect.Right, _rect.Bottom, _backgroundPaint);
            _canvas.DrawTextSafe(maxText, _rect.Right - 20 - textWidth, _rect.Bottom + 1 + _textPaint.FontMetrics.Bottom / 2.0f, _textPaint);

        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            _textPaint.Dispose();

            base.Dispose(disposing);
        }
    }
}
