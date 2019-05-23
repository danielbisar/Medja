using System;
using System.Diagnostics;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SkiaSharp;

namespace SkiaSharp_RenderingPerformance
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
        
        private SKColor[] _colors = { SKColors.Blue, SKColors.Green, SKColors.Aquamarine, SKColors.Lavender, SKColors.Brown, SKColors.Honeydew,  };
        private int _framesRendered = 0;
        private Stopwatch _stopwatch = Stopwatch.StartNew();
        const float w = 3;
        const float h = 3;

        private int _windowWidth = 800;
        private int _windowHeight = 600;

        private bool _needsRedraw;
        
        public Program()
        {
            _gameWindow = new GameWindow();
        }

        private void Run()
        {
            _gameWindow.Load += OnWindowLoad;
            _gameWindow.Resize += OnWindowResize;
            _gameWindow.UpdateFrame += OnRenderAndUpdateFrame;
            _gameWindow.Width = _windowWidth;
            _gameWindow.Height = _windowHeight;
            _gameWindow.Run();

            _canvas?.Dispose();
            _skSurface?.Dispose();
            _renderTarget?.Dispose();
            _grContext?.Dispose();
        }
        
        private void OnRenderAndUpdateFrame(object sender, FrameEventArgs e)
        {
            // draw 2 seems to be the best option
            
            Draw2();
        }

        private void Draw1()
        {
            _framesRendered++;

            if (_stopwatch.ElapsedMilliseconds > 2000)
            {
                Console.WriteLine("FPS: " + _framesRendered / (_stopwatch.ElapsedMilliseconds / 1000.0f));

                _framesRendered = 0;
                _stopwatch.Restart();
            }

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Green;
                paint.IsAntialias = true;

                _canvas.Clear();

                for (int i = 0; i < 55000; i++)
                {
                    var w2 = 2.0f * w;
                    var x = (i * w2) % (_windowWidth - w);
                    var y = (i / (_windowWidth / w2)) * w2;

                    DrawCircle(x, y, paint, i);
                }
            }

            _canvas.Flush();
            _gameWindow.SwapBuffers();
        }

        private void Draw2()
        {
            _framesRendered++;

            if (_stopwatch.ElapsedMilliseconds > 2000)
            {
                Console.WriteLine("FPS: " + _framesRendered / (_stopwatch.ElapsedMilliseconds / 1000.0f));

                _framesRendered = 0;
                _stopwatch.Restart();
            }

            if (_needsRedraw)
            {
                using (var paint = new SKPaint())
                {
                    paint.Color = SKColors.Green;
                    paint.IsAntialias = true;

                    _canvas.Clear();

                    for (int i = 0; i < 55000; i++)
                    {
                        var w2 = 2.0f * w;
                        var x = (i * w2) % (_windowWidth - w);
                        var y = (i / (_windowWidth / w2)) * w2;

                        DrawCircle(x, y, paint, i);
                    }
                }

                _canvas.Flush();
                _gameWindow.SwapBuffers();

                _needsRedraw = false;
            }
        }

        private SKImage _surfaceSnapshot;

        private void Draw3()
        {
            _framesRendered++;

            if (_stopwatch.ElapsedMilliseconds > 2000)
            {
                Console.WriteLine("FPS: " + _framesRendered / (_stopwatch.ElapsedMilliseconds / 1000.0f));

                _framesRendered = 0;
                _stopwatch.Restart();
            }

            if (_needsRedraw)
            {
                using (var paint = new SKPaint())
                {
                    paint.Color = SKColors.Green;
                    paint.IsAntialias = true;

                    _canvas.Clear();

                    for (int i = 0; i < 55000; i++)
                    {
                        var w2 = 2.0f * w;
                        var x = (i * w2) % (_windowWidth - w);
                        var y = (i / (_windowWidth / w2)) * w2;

                        DrawCircle(x, y, paint, i);
                    }

                    _canvas.Flush();
                    
                    _surfaceSnapshot?.Dispose();
                    _surfaceSnapshot = _skSurface.Snapshot();
                }

                _needsRedraw = false;
            }

            if (_surfaceSnapshot != null)
            {
                _canvas.DrawImage(_surfaceSnapshot, new SKPoint(), new SKPaint()
                {
                    IsAntialias = false,
                    HintingLevel = SKPaintHinting.Full,
                    FilterQuality = SKFilterQuality.High
                });
                _canvas.Flush();
                _gameWindow.SwapBuffers();
            }
        }

        private void DrawCircle(float x, float y, SKPaint paint, int color)
        {
            var halfW = w / 2.0f;
            paint.Color = _colors[color % _colors.Length];
            _canvas.DrawCircle(x + halfW, y + halfW, w, paint);
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            _windowWidth = clientRect.Width;
            _windowHeight = clientRect.Height;
            
            GL.Viewport(0, 0, _windowWidth, _windowHeight);
            
            _canvas?.Dispose();
            _skSurface?.Dispose();
            
            _renderTarget?.Dispose();
            _renderTarget = CreateRenderTarget(_windowWidth, _windowHeight);
            
            _skSurface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            _canvas = _skSurface.Canvas;
            _grContext.ResetContext();

            _needsRedraw = true;
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