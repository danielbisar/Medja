using System;
using System.Collections.Generic;
using System.Threading;
using Medja.Controls;
using Medja.Controls.Images;
using Medja.OpenTk.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Medja.OpenTk.Input;
using Medja.Theming;
using Medja.Utils;
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
        private FramesPerSecondLimiter _frameLimiter;

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
            RendererFactory = () => new OpenTk2DOnlyRenderer();
            _focusManager = FocusManager.Default;
            _controls = new List<Control>();
            TaskQueue = new TaskQueue<object>();
            BitmapFactory = new SkiaBitmapFactory();
        }

        /// <inheritdoc />
        public void Run(MedjaApplication application, int maxFps)
        {
            _medjaWindow = application.MainWindow;
            _gameWindow = ((OpenTkWindow)_medjaWindow).GameWindow;
            _controlHierarchy = new ControlHierarchy(_medjaWindow);
            _frameLimiter = new FramesPerSecondLimiter(maxFps, UpdateAndRender);

            using (_gameWindow)
            {
                _gameWindow.Resize += OnResize;
                _gameWindow.UpdateFrame += OnUpdateFrame;
                _gameWindow.Closed += OnWindowClosed;
                _gameWindow.Load += OnWindowLoad;

                _mouseHandler = new OpenTkMouseHandler(_medjaWindow, _gameWindow, _focusManager);
                _mouseHandler.Controls = _controls;

                _keyboardHandler = new OpenTKKeyboardHandler(_gameWindow, _focusManager);

                _frameLimiter.Run();
                // do not limit fps via this method, since OpenTK keeps using too much CPU
                _gameWindow.Run();
            }
        }

        private void OnWindowLoad(object sender, EventArgs e)
        {
            _renderer = RendererFactory();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _renderer?.Dispose();

//            _mouseHandler.Dispose();
//            _keyboardHandler.Dispose();
//            _focusManager.Dispose();
            // todo ? BitmapFactory.Dispose();
            TaskQueue.Dispose();
            _medjaWindow.Dispose();
            _frameLimiter.Dispose();
        }

        private void OnResize(object sender, EventArgs e)
        {
            var clientRect = _gameWindow.ClientRectangle;
            GL.Viewport(0, 0, clientRect.Width, clientRect.Height);

            _renderer.SetSize(clientRect);
        }
        
        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            // calls render via the limiter, waits if the we are too fast
            // but continues always if we are too slow
            // uses much less CPU than the OpenTK implementation
            _frameLimiter.Render();  // will call this.UpdateAndRender()
        }

        private void UpdateAndRender()
        {
            // todo could be optimized, when checking for _control.Parent changes
            // and only render if NeedsRender, IsLayoutUpdate or Parent changes
            
            // update controls every frame (f.e. a control was hidden and is now visible)
            _controls.Clear();
            _controls.AddRange(_controlHierarchy.GetInRenderingOrder());

            _controlHierarchy.UpdateLayout();

            // executes all task requested by anyone in the requested order
            TaskQueue.ExecuteAll();

            if (_renderer.Render(_controls))
            {
                // display what was just drawn
                _gameWindow.SwapBuffers();
            }
        }
    }
}
