using System;
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
            // todo translate Y coordinate for GetForDrawing
            
            // create an array and use DrawPoints reduces the amount of methods called and is faster
            // than calling DrawPoint for each point
            
            /* without aggregation and xMinDist = 1
             FPS: 0.9225093
             Point count: 13173427 */
            
            /* with aggregation and xMinDist = 1
             FPS: 3.69
             Point count: 799 */
            
            var pointsToDraw = _control.DataPoints.Downsampler.Downsample(borderRect.Left, borderRect.Right, 0.1f, 0.2f);
            //var pointsToDraw = _control.DataPoints.GetAll();
            
            //Console.WriteLine("Point count:" + pointsToDraw.Count);
            
            // _canvas.DrawPoint is faster than creating SKPoints and call _canvas.DrawPoints
            
            
            for(int i = 0; i < pointsToDraw.Count; i++)
            {
                var dataPoint = pointsToDraw[i];
                _canvas.DrawPoint(dataPoint.X, dataPoint.Y + 300, _pointsPaint);
            }
            
        }
    }
}