using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Medja.Controls;
using Medja.Layers;
using Medja.Layers.Layouting;
using Medja.OpenTk.Eval;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    // TODO dispose
    public class OpenTkRenderLayer : ILayer, IDisposable
    {
        private SkiaGLLayer _skia;
        private SKSurface _surface;
        private SKPaint _paint;
        private SKCanvas _canvas;

        public OpenTkRenderLayer()
        {
            _skia = new SkiaGLLayer();
        }

        public void Resize(int width, int height)
        {
            _skia.Resize(width, height);
        }

        public IEnumerable<ControlState> Apply(IEnumerable<ControlState> states)
        {
            using (_surface = _skia.CreateSurface())
            {
                using (_paint = new SKPaint
                {
                    // TODO dispose Typeface
/*                    Typeface = SKTypeface.FromFamilyName("Arial"),
                    TextSize = 12,*/
                    IsAntialias = true,
                    Color = SKColors.Orange,
                    Style = SKPaintStyle.StrokeAndFill,
                    //StrokeWidth = 10
                })
                {
                    using (_canvas = _surface.Canvas)
                    {
                        _canvas.Clear(SKColors.Gray);

                        Debug.WriteLine(" --- OpenTkRenderLayer --- ");

                        foreach (var state in states)
                        {
                            var item = state.Control;
                            var position = state.Position;

                            Debug.WriteLine(" - Control: " + item.GetType().Name);
                            Debug.WriteLine("      - Position: " + position);

                            if (item is Button b)
                            {
                                DrawRect(position);
                                var oldColor = _paint.Color;
                                _paint.Color = SKColors.Black;
                                DrawTextCenteredInRect(position, b.Text);
                                _paint.Color = oldColor;
                            }
                            else
                                DrawRect(position);
                        }

                        _canvas.Flush();
                    }
                }
            }

            return states;
        }

        private void DrawTextCenteredInRect(PositionInfo position, string text)
        {
            //var width = _paint.MeasureText(text);
            //var height = _paint.TextSize;*/

            // work with a copy so we don't change the source PositionInfo
            var skPoint = new SKPoint(position.X + position.Width / 2, position.Y + position.Height / 2); 
            
            _canvas.DrawText(text, skPoint.X, skPoint.Y, _paint);
        }

        private void DrawText(PositionInfo positionInfo, string text)
        {
            _canvas.DrawText(text, positionInfo.X, positionInfo.Y, _paint);
        }

        private void DrawRect(PositionInfo positionInfo)
        {
            _canvas.DrawRect(GetSKRectFrom(positionInfo), _paint);
        }

        private SKRect GetSKRectFrom(PositionInfo positionInfo)
        {
            return new SKRect(positionInfo.X, positionInfo.Y, positionInfo.Width + positionInfo.X, positionInfo.Height + positionInfo.Y);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_paint != null)
                        _paint.Dispose();

                    if (_surface != null)
                        _surface.Dispose();        

                    if (_skia != null)
                        _skia.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OpenTkRenderLayer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
