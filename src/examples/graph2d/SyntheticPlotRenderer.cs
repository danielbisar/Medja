using System;
using Medja.Controls;
using Medja.Controls.Graph2D;
using Medja.OpenTk.Themes;
using Medja.Primitives;
using SkiaSharp;

namespace graph2d
{
    internal sealed class SyntheticPlotRenderer : SkiaControlRendererBase<Control>
    {
        private readonly DataPoints _data = new DataPoints();
        private readonly SKPaint _line = new SKPaint
        {
            Color = SKColors.OrangeRed, IsAntialias = true,
            Style = SKPaintStyle.Stroke, StrokeWidth = 2
        };
        private readonly SKPaint _axis = new SKPaint
        {
            Color = SKColors.LightGray, IsAntialias = true, TextSize = 14
        };

        public SyntheticPlotRenderer(Control control) : base(control)
        {
            for (var i = 0; i <= 256; i++)
            {
                var x = (float) (4 * Math.PI * i / 256);
                _data.Add(new Point(x, (float) Math.Sin(x)));
            }
        }

        protected override void InternalRender()
        {
            var left = _rect.Left + 35;
            var top = _rect.Top + 20;
            var width = _rect.Width - 55;
            var height = _rect.Height - 50;
            if (width <= 0 || height <= 0)
                return;

            _canvas.DrawLine(left, top, left, top + height, _axis);
            _canvas.DrawLine(left, top + height / 2, left + width, top + height / 2, _axis);
            _canvas.DrawText("1", left - 25, top + 10, _axis);
            _canvas.DrawText("-1", left - 25, top + height, _axis);
            _canvas.DrawText("0", left, top + height + 20, _axis);
            _canvas.DrawText("4 pi", left + width - 25, top + height + 20, _axis);

            using (var path = new SKPath())
            {
                for (var i = 0; i < _data.Count; i++)
                {
                    var point = _data.Points[i];
                    var x = left + width * point.X / (float) (4 * Math.PI);
                    var y = top + height * (1 - point.Y) / 2;
                    if (i == 0)
                        path.MoveTo(x, y);
                    else
                        path.LineTo(x, y);
                }
                _canvas.DrawPath(path, _line);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _line.Dispose();
            _axis.Dispose();
            base.Dispose(disposing);
        }
    }
}
