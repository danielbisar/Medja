using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace MultiOpenGLContext
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }
        
        private readonly GameWindow _gameWindow;
        private readonly GlContextManager _contextManager;
        
        private GlObject _glObject;
        private int _programId;

        private GlObject _glObject2;
        private int _programId2;

        private GlObject _glObject3;
        private int _programId3;

        public Program()
        {
            _gameWindow = new GameWindow(800,600,
                GraphicsMode.Default, "", GameWindowFlags.Default, 
                DisplayDevice.Default, 
                4, 2, GraphicsContextFlags.ForwardCompatible);
            
            _contextManager = new GlContextManager(_gameWindow.WindowInfo, _gameWindow.Context);
            
            GRContext skiaGrContext = null;
            SKSurface surface = null;
            GRBackendRenderTarget renderTarget = null;
            SKCanvas canvas = null;
            
            var skiaContext = _contextManager.WindowContext;
            
            skiaContext.Actions.OnInit = () =>
            {
                skiaGrContext = GRContext.CreateGl();
                GL.ClearColor(0.3f, 0.1f, 0.1f, 1);
            };
            skiaContext.Actions.OnResize = (w,h) =>
            {
                GL.Viewport(0, 0, w, h);

                if (skiaGrContext != null)
                {
                    renderTarget?.Dispose();
                    renderTarget = CreateRenderTarget(w, h);

                    canvas?.Dispose();
                    surface?.Dispose();

                    surface = SKSurface.Create(skiaGrContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

                    if(surface != null)
                        canvas = surface.Canvas;
                    
                    skiaGrContext.ResetContext();
                }
            };
            skiaContext.Actions.OnRender = () =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                if (canvas != null)
                {
                    using (var paint = new SKPaint())
                    {
                        paint.StrokeWidth = 2;
                        paint.IsStroke = true;
                        paint.IsAntialias = true;
                        
                        canvas.DrawLine(0, 0, 100, 600, paint);
                        canvas.Flush();
                    }
                }
            };

            var context2 = _contextManager.CreateWindowContext();
            context2.Actions.OnResize = (w, h) =>
            {
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(10, 10, 200, 200);
                GL.Viewport(10, 10, 200, 200);
            };
            context2.Actions.OnInit = () =>
            {
                _programId2 = ShaderFactory.CreateShaderProgram();
                _glObject2 = new GlObject(new[]
                {
                    new Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.0f, -0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                });

                GL.ColorMask(true, true, true, true);

//                GL.ClearColor(0f, 0.8f, 0.1f, 1);
                GL.ClearColor(0f, 0.0f, 0.0f, 0);
                GL.Enable(EnableCap.ScissorTest);
            };
            context2.Actions.OnRender = () =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(_programId2);
                _glObject2.Render();
            };

            var context3 = _contextManager.CreateWindowContext();
            context3.Actions.OnResize = (w, h) =>
            {
                GL.Scissor(w-210, 10, 200, 200);
                GL.Viewport(w-210, 10, 200, 200);
            };
            context3.Actions.OnInit = () =>
            {
                _programId3 = ShaderFactory.CreateShaderProgram();
                _glObject3 = new GlObject(new[]
                {
                    new Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.0f, -0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                });

                GL.ClearColor(0f, 0.2f, 0.8f, 1);
                GL.Enable(EnableCap.ScissorTest);
            };
            context3.Actions.OnRender = () =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(_programId3);
                _glObject3.Render();
            };
            
            _gameWindow.Resize += OnResize;
            _gameWindow.RenderFrame += OnRender;
            _gameWindow.Load += OnLoad;
            _gameWindow.Closed += OnClosed;
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
            //GL.GetInteger(GetPName.StencilBits, out var stencilBits);

            var glInfo = new GRGlFramebufferInfo((uint) fboId, SKColorType.Rgba8888.ToGlSizedFormat());
            var renderTarget = new GRBackendRenderTarget(width, height, sampleCount, 0, glInfo);

            return renderTarget;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _contextManager.Dispose();
        }

        public void Run()
        {
            _gameWindow.Run();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _contextManager.Init();
        }

        private void OnResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            
            _contextManager.Resize(clientRect.Width, clientRect.Height);
            _gameWindow.MakeCurrent();
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            _contextManager.Render();
        }
    }
}
