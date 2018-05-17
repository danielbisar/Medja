using System;
using System.Linq;
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
		private IRenderer _renderer;
        private OpenTkMouseHandler _mouseHandler;

        public ControlFactory ControlFactory { get; }

        /// <summary>
        /// Gets or sets the function that is used to create the renderer. 
		/// The method is called after the window is displayed.
        /// </summary>
        /// <value>The renderer factory.</value>
		public Func<IRenderer> RendererFactory { get; set; }

		public MedjaOpenTkLibrary(ControlFactory factory = null)
        {
			ControlFactory = factory ?? new OpenTkTheme();
			RendererFactory = () => new OpenTkRenderer();
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

            var controls = _medjaWindow.GetAllControls().ToList();
            _renderer.Render(controls);

            // display what was just drawn
            _gameWindow.SwapBuffers();
        }

        private void AssureRenderer()
        {
            if (_renderer == null)
				_renderer = RendererFactory(); // TODO cannot be initialized without the window, but should also not have the null check here every call, for now seems fine
        }
    }
}
