using System;
using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// This class is the main entry point for the rendering of controls for OpenTk with Skia.
    /// </summary>
    public class OpenTkRenderer : IDisposable
    {
        private SkiaGLLayer _skia;
        private SKSurface _surface;
        private SKPaint _paint;
        private SKCanvas _canvas;
        private bool _isDisposed;

        public OpenTkRenderer()
        {
            _skia = new SkiaGLLayer();
        }

        public void SetSize(Rectangle rectangle)
        {
            _skia.Resize(rectangle.Width, rectangle.Height);
        }

        public void Render(IEnumerable<Control> controls)
        {
            var controls3d = new List<Control3D>();
            var controls2d = new List<Control>();

            foreach(var control in controls)
            {
                if (control is Control3D control3d)
                    controls3d.Add(control3d);
                else
                    controls2d.Add(control);
            }

            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.UseProgram(0);

            foreach (var control3d in controls3d)
                control3d.Render();
            
            //GL.PushClientAttrib(ClientAttribMask.ClientAllAttribBits);

            _surface = _skia.CreateSurface();
            _canvas = _surface.Canvas;
            _paint = GetDefaultPaint();

            //_canvas.Clear(SKColors.Gray);

            foreach (var control in controls2d)
            {
                Render(control);
            }

            _canvas.Flush();
            _canvas.Dispose();
            _paint.Dispose();
            _surface.Dispose();

            //GL.PopClientAttrib();
        }

        private void Render(Control control)
        {
            if (control is Button b)
            {
                var position = control.Position;
                var skRect = position.ToSKRect();

                var oldColor = _paint.Color;

                _paint.Color = control.Background.ToSKColor();                
                _canvas.DrawRect(skRect, _paint);

                _paint.Color = SKColors.Black;
                DrawTextCenteredInRect(skRect, b.Text);
                _paint.Color = oldColor;
            }
            //else
            //    DrawRect(position);
        }

        private SKPaint GetDefaultPaint()
        {
            return new SKPaint
            {
                // TODO dispose Typeface
                /*                    Typeface = SKTypeface.FromFamilyName("Arial"),
                                    TextSize = 12,*/
                IsAntialias = true,
                Color = SKColors.Orange,
                Style = SKPaintStyle.StrokeAndFill,
                //StrokeWidth = 10
            };
        }

        private void DrawTextCenteredInRect(SKRect rect, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var width = _paint.MeasureText(text);
            var height = _paint.TextSize;

            _canvas.DrawText(text, rect.MidX - width / 2, rect.MidY + height / 2, _paint);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
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

                _isDisposed = true;
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
