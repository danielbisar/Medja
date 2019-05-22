using System;
using System.Collections.Generic;
using Medja.Controls;
using Medja.Controls.Images;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Medja.OpenTk.Input;
using Medja.Theming;
using Medja.Utils.Threading.Tasks;

namespace Medja.OpenTk
{
    /// <summary>
    /// Implements the connection between OpenTk and Medja.
    /// </summary>
    public class MedjaOpenTkLibrary : IMedjaLibrary
    {
        private readonly FocusManager _focusManager;
        private readonly List<Control> _controls;

        private MedjaWindow _medjaWindow;
        private GameWindow _gameWindow;
        private IRenderer _renderer;
        private OpenTkMouseHandler _mouseHandler;
        private OpenTKKeyboardHandler _keyboardHandler;
        private ControlHierarchy _controlHierarchy;

        public IControlFactory ControlFactory { get; }

        public BitmapFactory BitmapFactory { get; }

        public TaskQueue<object> TaskQueue { get; }

        /// <summary>
        /// Gets or sets the function that is used to create the renderer. 
        /// The method is called after the window is displayed.
        /// </summary>
        /// <value>The renderer factory.</value>
        public Func<IRenderer> RendererFactory { get; set; }

        public MedjaOpenTkLibrary(IControlFactory factory)
        {
            ControlFactory = factory;
            RendererFactory = () => new OpenTkRenderer();
            _focusManager = FocusManager.Default;
            _controls = new List<Control>();
            TaskQueue = new TaskQueue<object>();
            BitmapFactory = new SkiaBitmapFactory();
        }

        /// <inheritdoc />
        public void Run(MedjaApplication application)
        {
            _medjaWindow = application.MainWindow;
            _gameWindow = ((OpenTkWindow)_medjaWindow).GameWindow;
            _controlHierarchy = new ControlHierarchy(_medjaWindow);

            using (_gameWindow)
            {
                _gameWindow.Resize += OnResize;
                _gameWindow.UpdateFrame += OnUpdateFrame;
                _gameWindow.RenderFrame += OnRenderFrame;
                _gameWindow.Closed += OnWindowClosed;

                _mouseHandler = new OpenTkMouseHandler(_medjaWindow, _gameWindow, _focusManager);
                _mouseHandler.Controls = _controls;

                _keyboardHandler = new OpenTKKeyboardHandler(_gameWindow, _focusManager);

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

//            _mouseHandler.Dispose();
//            _keyboardHandler.Dispose();
//            _focusManager.Dispose();
            // todo ? BitmapFactory.Dispose();
            TaskQueue.Dispose();
            _medjaWindow.Dispose();
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
            // update controls every frame (f.e. a control was hidden and is now visible)
            _controls.Clear();
            _controls.AddRange(_controlHierarchy.GetInRenderingOrder());

            _controlHierarchy.UpdateLayout();

            // executes all task requested by anyone in the requested order
            TaskQueue.ExecuteAll();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            AssureRenderer();

            _renderer.Render(_controls);

            // display what was just drawn
            _gameWindow.SwapBuffers();
        }

        private void AssureRenderer()
        {
            if (_renderer == null)
                _renderer = RendererFactory();
        }
    }
}
