using System;
using System.Linq;
using Medja.Controls;
using Medja.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class Graph2DRenderer : SkiaControlRendererBase<Graph2D>
    {
        private readonly SKPaint _pointsPaint;
        private readonly FramesPerSecondCounter _counter;

        public Graph2DRenderer(Graph2D control)
            : base(control)
        {
            _pointsPaint = CreatePaint();
            _pointsPaint.Color = SKColors.Green;
            _pointsPaint.IsAntialias = false;

            _counter = new FramesPerSecondCounter();
            _counter.FramesCounted += (s, e) => Console.WriteLine("FPS: " + e.Value);
        }

        protected override void InternalRender()
        {
            _counter.Tick();
            
            RenderValues(_rect);
        }
        
        private void RenderValues(SKRect borderRect)
        {
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.1f, 0.1f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.5f, 1f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.5f, 0.1f);
            var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 3.0f, 0.2f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0, 0);
            
            //Console.WriteLine(pointsToDraw.Count);
            
            var points = pointsToDraw.Select(p => new SKPoint(p.X, p.Y + 300)).ToArray();
            _canvas.DrawPoints(SKPointMode.Lines, points, _pointsPaint);
        }
    }
}