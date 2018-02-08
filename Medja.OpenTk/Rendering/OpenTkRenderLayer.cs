using System;
using Medja.Controls;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class OpenTkRenderLayer : ILayer, IDisposable
    {
        private SkiaGLLayer _skia;
        private SKSurface _surface;
        private SKPaint _paint;
        private SKCanvas _canvas;

        public WorkflowState WorkflowState { get; set; }

        public OpenTkRenderLayer()
        {
            _skia = new SkiaGLLayer();
        }

        public void Execute()
        {
            var renderTargetSize = WorkflowState.RenderTargetSize;
            // TODO check performance
            _skia.Resize((int)renderTargetSize.Width, (int)renderTargetSize.Height);

            /* Done via Skia GL.ClearColor(Color.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);*/
            
            _surface = _skia.CreateSurface();
            _canvas = _surface.Canvas;
            _paint = GetDefaultPaint();

            _canvas.Clear(SKColors.Gray);

            foreach (var controlState in WorkflowState.Controls)
            {
                Render(controlState);
            }

            _canvas.Flush();
            _paint.Dispose();
            _surface.Dispose();


            // TODO for 3D
            /*GL.UseProgram(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);*/
                        
            //GL.Rotate(2, 1, 1, 0);
            //GL.Begin(BeginMode.LineLoop);
            //GL.Vertex3(0, 0, 0);
            //GL.Vertex3(0, 0.5, 0);
            //GL.Vertex3(0.5, 0.5, 0.5);
            //GL.Vertex3(0.5, 0, 0.5);
            //GL.End();
        }

        private void Render(ControlState controlState)
        {
            var control = controlState.Control;
            var position = controlState.Position;

            if (control is Button b)
            {
                var oldColor = _paint.Color;

                _paint.Color = controlState.InputState.IsMouseOver ? SKColors.DarkOrange : SKColors.Orange;
                DrawRect(position);
                
                _paint.Color = SKColors.Black;
                DrawTextCenteredInRect(position, b.Text);
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

        private void DrawTextCenteredInRect(Position position, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            //var width = _paint.MeasureText(text);
            //var height = _paint.TextSize;*/

            // work with a copy so we don't change the source PositionInfo
            var skPoint = new SKPoint(position.X + position.Width / 2, position.Y + position.Height / 2);

            _canvas.DrawText(text, skPoint.X, skPoint.Y, _paint);
        }

        private void DrawText(Position positionInfo, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            _canvas.DrawText(text, positionInfo.X, positionInfo.Y, _paint);
        }

        private void DrawRect(Position positionInfo)
        {
            _canvas.DrawRect(GetSKRectFrom(positionInfo), _paint);
        }

        private SKRect GetSKRectFrom(Position positionInfo)
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
