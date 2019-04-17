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
        private readonly FramesPerSecondCounter _fpsCounter;

        public Graph2DRenderer(Graph2D control)
            : base(control)
        {
            _pointsPaint = CreatePaint();
            _pointsPaint.Color = SKColors.Green;
            _pointsPaint.IsAntialias = false;

            _fpsCounter = new FramesPerSecondCounter(5);
            _fpsCounter.FramesCounted += (s, e) => Console.WriteLine("2D Graph FPS " + e.Value.ToString("F2"));
        }

        protected override void InternalRender()
        {
            _fpsCounter.Tick();
            RenderValues(_rect);
        }

        int i = 0;
        
        private void RenderValues(SKRect borderRect)
        {
            i++;            

            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.1f, 0.1f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.5f, 1f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.5f, 0.1f);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 3.0f, 0.2f);
            var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, _control.MinDistance, _control.SlopThreshold);
            //var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0, 0);
            
            var div = _fpsCounter.FramesPerSecond;
            if(div > 0)
            {
            if(i % (int)(div*5) < 1)
            {
                Console.WriteLine("Points: " + pointsToDraw.Count);
                i = 1;
            }
            }
            //Console.WriteLine(pointsToDraw.Count);
            
            var points = pointsToDraw.Select(p => new SKPoint(p.X, p.Y + 300)).ToArray();

            //if(_control.DataPoints.Downsampler is DummyDownsampler)
                _canvas.DrawPoints((SKPointMode)_control.RenderMode, points, _pointsPaint);
            //else
            //    _canvas.DrawPoints(SKPointMode.Lines, points, _pointsPaint);
        }
    }
}