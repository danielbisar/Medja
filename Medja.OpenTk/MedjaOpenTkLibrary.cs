using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk
{
    public class MedjaOpenTkLibrary : IMedjaLibrary
    {
        private MedjaWindow _medjaWindow;
        private GameWindow _gameWindow;

        public void Run(MedjaApplication application)
        {
            _medjaWindow = application.MainWindow;
            _gameWindow = ((OpenTkWindow)_medjaWindow).GameWindow;

            using (_gameWindow)
            {
                _gameWindow.Resize += OnResize;
                _gameWindow.UpdateFrame += OnUpdateFrame;
                _gameWindow.RenderFrame += OnRenderFrame;

                _gameWindow.Run(1 / 30.0);
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            GL.Viewport(0, 0, clientRect.Width, clientRect.Height);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            _medjaWindow.UpdateLayout();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            _renderer.Render(_medjaWindow.GetAllControls());
        }

        public MedjaWindow CreateWindow()
        {
            return new OpenTkWindow();
        }
    }
}
