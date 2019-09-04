using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

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

            _contextManager = new GlContextManager(_gameWindow.WindowInfo);
            
            var context1 = _contextManager.Add(_gameWindow.Context);
            context1.Actions.OnResize = (w,h) =>
            {
                GL.Viewport(0, 0, w, h);
            };
            context1.Actions.OnInit = () =>
            {
                _programId = ShaderFactory.CreateShaderProgram();
                _glObject = new GlObject(new[]
                {
                    new Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.0f, -0.25f, 0.5f, 1f), Color4.Black),
                    new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1f), Color4.Black),
                });

                GL.ClearColor(0.3f, 0.1f, 0.1f, 1);
            };
            context1.Actions.OnRender = () =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(_programId);
                _glObject.Render();
            };

            var context2 = _contextManager.Create();
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

                GL.ClearColor(0f, 0.8f, 0.1f, 1);
                GL.Enable(EnableCap.ScissorTest);
            };
            context2.Actions.OnRender = () =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(_programId2);
                _glObject2.Render();
            };

            var context3 = _contextManager.Create();
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