using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Theming;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk
{
    /// <summary>
    /// Implements the connection between OpenTk and Medja.
    /// </summary>
    public class MedjaOpenTkLibrary : IMedjaLibrary
    {
        private MedjaWindow _medjaWindow;
        private GameWindow _gameWindow;
        private OpenTkRenderer _renderer;
        private OpenTkMouseHandler _mouseHandler;

        public ControlFactory ControlFactory { get; }

        public MedjaOpenTkLibrary()
        {
            ControlFactory = new DefaultTheme();
        }

        /// <inheritdoc />
        public MedjaWindow CreateWindow()
        {
            return new OpenTkWindow();
        }

        /// <inheritdoc />
        public void Run(MedjaApplication application)
        {
            _medjaWindow = application.MainWindow;
            _gameWindow = ((OpenTkWindow)_medjaWindow).GameWindow;

            using (_gameWindow)
            {
                _gameWindow.Resize += OnResize;
                _gameWindow.UpdateFrame += OnUpdateFrame;
                _gameWindow.RenderFrame += OnRenderFrame;
                _gameWindow.Closed += OnWindowClosed;

                _mouseHandler = new OpenTkMouseHandler(_medjaWindow, _gameWindow);

                _gameWindow.Run(1 / 30.0);
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.Dispose();
                _renderer = null;
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            GL.Viewport(0, 0, clientRect.Width, clientRect.Height);

            AssureRenderer();
            _renderer.SetSize(clientRect);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            _medjaWindow.UpdateLayout();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            AssureRenderer();            

            var controls = _medjaWindow.GetAllControls();
            _renderer.Render(controls);

            // display what was just drawn
            _gameWindow.SwapBuffers();
        }

        private void AssureRenderer()
        {
            if (_renderer == null)
                _renderer = new OpenTkRenderer(); // TODO cannot be initialized without the window, but should also not have the null check here every call, for now seems fine
        }
    }
}
