using System;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SkiaSharp;

namespace SkiaSharp_CanvasPartialClear
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private GameWindow _gameWindow;
        private GRContext _grContext;
        private GRBackendRenderTarget _renderTarget;
        private SKSurface _skSurface;
        private SKCanvas _canvas;
        
        public Program()
        {
            _gameWindow = new GameWindow();
        }

        private void Run()
        {
            _gameWindow.Load += OnWindowLoad;
            _gameWindow.Resize += OnWindowResize;
            _gameWindow.UpdateFrame += OnRenderAndUpdateFrame;
            _gameWindow.MouseDown += OnWindowMouseDown;
            _gameWindow.Run();

            _canvas?.Dispose();
            _skSurface?.Dispose();
            _renderTarget?.Dispose();
            _grContext?.Dispose();
        }

        private int _redrawRectIndex = -1;
        private SKColor[] _colors = { SKColors.Blue, SKColors.Green, SKColors.Aquamarine, SKColors.Black, SKColors.Brown,  };
        private bool _redraw = false;

        private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Window Mouse Down");
            _redraw = true;
        }
        
        private void OnRenderAndUpdateFrame(object sender, FrameEventArgs e)
        {
            // reduce CPU load
            //Thread.Sleep(10);
            Draw();
        }

        private void Draw()
        {
            if (!_redraw)
                return;
            
            Console.WriteLine("Draw");

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Green;
                paint.IsAntialias = true;

                if (_redrawRectIndex < 0)
                {
                    _canvas.Clear(SKColors.Beige);

                    DrawRect(10, 10, 100, 100, paint);
                    DrawRect(120, 10, 100, 100, paint);
                    DrawRect(230, 10, 100, 100, paint);
                }
                else
                {
                    //_canvas.Clear(SKColors.Beige);
                    DrawRect(120, 10, 100, 100, paint);
                }

                _canvas.Flush();
                _gameWindow.SwapBuffers();
            }

            _redraw = false;
        }

        private void DrawRect(float x, float y, float w, float h, SKPaint paint)
        {
            _redrawRectIndex++;
            paint.Color = _colors[_redrawRectIndex % _colors.Length];
            _canvas.DrawRect(x, y, w, h, paint);
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            GL.Viewport(0, 0, clientRect.Width, clientRect.Height);
            
            _canvas?.Dispose();
            _skSurface?.Dispose();
            
            _renderTarget?.Dispose();
            _renderTarget = CreateRenderTarget(clientRect.Width, clientRect.Height);
            
            _skSurface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            _canvas = _skSurface.Canvas;

        }

        private void OnWindowLoad(object sender, EventArgs e)
        {
            _grContext = GRContext.CreateGl();
        }

        /// <summary>
        /// Creates the <see cref="GRBackendRenderTarget"/> based on the current OpenGL values.
        /// </summary>
        /// <param name="width">The width the target should have.</param>
        /// <param name="height">The height the target should have.</param>
        /// <returns>The <see cref="GRBackendRenderTarget"/>.</returns>
        private GRBackendRenderTarget CreateRenderTarget(int width, int height)
        {
            GL.GetInteger(GetPName.FramebufferBinding, out var fboId);
            GL.GetInteger(GetPName.Samples, out var sampleCount);
            GL.GetInteger(GetPName.StencilBits, out var stencilBits);

            var glInfo = new GRGlFramebufferInfo((uint) fboId, SKColorType.Rgba8888.ToGlSizedFormat());
            var renderTarget = new GRBackendRenderTarget(width, height, sampleCount, stencilBits, glInfo);

            return renderTarget;
        }
    }
}