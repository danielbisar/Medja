using System;
using System.Collections.Generic;
using Medja.Controls;
using Medja.OpenTk.Input;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using Medja.Properties;
using Medja.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk
{
    public class OpenTkWindow : Window
    {
        private readonly List<Control> _controls;

        private OpenTkRenderer _renderer;
        private OpenTkMouseHandler _mouseHandler;
        private OpenTkKeyboardHandler _keyboardHandler;
        private ControlHierarchy _controlHierarchy;
        private FramesPerSecondLimiter _frameLimiter;
        
        private bool _calledClose;
        private bool _selfUpdatePosition;

        public GameWindow GameWindow { get; }
        
        public OpenTkWindow(MedjaOpenTKWindowSettings windowSettings)
        : base(windowSettings.ControlFactory)
        {
            _controls = new List<Control>();
            
            var openGLVersion = windowSettings.OpenGLVersion;
            
            GameWindow = new GameWindow(800, 600, 
                GraphicsMode.Default, 
                "", 
                GameWindowFlags.Default, 
                DisplayDevice.Default, 
                openGLVersion.Major, openGLVersion.Minor, 
                GraphicsContextFlags.ForwardCompatible);
            
            _controlHierarchy = new ControlHierarchy(this);
            _frameLimiter = new FramesPerSecondLimiter(windowSettings.MaxFramesPerSecond, UpdateAndRender);
            
            // order of events
            // load, resize, move, render, closed
            GameWindow.Load += OnGameWindowLoad;
            GameWindow.Move += OnGameWindowMove;
            GameWindow.Resize += OnGameWindowResize;
            GameWindow.Closed += OnGameWindowClosed;
            GameWindow.RenderFrame += OnRender;

            GameWindow.Title = Title;
            Position.X = GameWindow.X;
            Position.Y = GameWindow.Y;
            Position.Height = GameWindow.Height;
            Position.Width = GameWindow.Width;

            PropertyState.PropertyChanged += OnStateChanged;
            PropertyTitle.PropertyChanged += OnTitleChanged;
            Position.PropertyX.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyY.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyWidth.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyHeight.PropertyChanged += OnPositionPropertyChanged;

            _mouseHandler = new OpenTkMouseHandler(this, GameWindow, FocusManager);
            _mouseHandler.Controls = _controls;

            _keyboardHandler = new OpenTkKeyboardHandler(GameWindow, FocusManager);
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            // calls render via the limiter, waits if the we are too fast
            // but continues always if we are too slow
            // uses much less CPU than the OpenTK implementation
            _frameLimiter.Render(); // will call UpdateAndRender()
        }

        private void OnGameWindowLoad(object sender, EventArgs e)
        {
            _renderer = new OpenTkRenderer(sender as GameWindow);
        }

        private void OnTitleChanged(object sender, PropertyChangedEventArgs e)
        {
            GameWindow.Title = Title;
        }

        private void OnStateChanged(object sender, PropertyChangedEventArgs e)
        {
            GameWindow.WindowState = State.ToOpenTKWindowState();
        }

        private void OnPositionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(_selfUpdatePosition)
                return;
            
            GameWindow.X = (int)Position.X;
            GameWindow.Y = (int) Position.Y;
            GameWindow.Width = (int) Position.Width;
            GameWindow.Height = (int) Position.Height;
        }

        private void OnGameWindowMove(object sender, EventArgs e)
        {
            _selfUpdatePosition = true;
            Position.X = GameWindow.X;
            Position.Y = GameWindow.Y;
            _selfUpdatePosition = false;
        }

        public override void Show()
        {
            _frameLimiter.Run();
            
            // do not limit fps via this method, since OpenTK keeps using too much CPU
            GameWindow.Run();
        }

        private void UpdateAndRender()
        {
            GameWindow.MakeCurrent();

            // todo could be optimized, when checking for _control.Parent changes
            // and only render if NeedsRender, IsLayoutUpdate or Parent changes

            // update controls every frame (f.e. a control was hidden and is now visible)
            _controls.Clear();
            _controls.AddRange(_controlHierarchy.GetInRenderingOrder());

            _controlHierarchy.UpdateLayout();

            // executes all task requested by anyone in the requested order
            TaskQueue.ExecuteAll();

            _renderer.Render(_controls);
        }

        private void OnGameWindowResize(object sender, EventArgs eventArgs)
        {
            _selfUpdatePosition = true;
            Position.Width = GameWindow.Width;
            Position.Height = GameWindow.Height;
            _selfUpdatePosition = false;

            GameWindow.MakeCurrent();
            
            // TODO extract OpenGL calls into strategy, so we can handle multiple versions?
            var clientRect = GameWindow.ClientRectangle;
            GL.Viewport(0, 0, clientRect.Width, clientRect.Height);

            _renderer.SetSize(clientRect);
        }

        private void OnGameWindowClosed(object sender, EventArgs e)
        {
            GameWindow.Closed -= OnGameWindowClosed;
            Close();
        }

        public override void Close()
        {
            // prevent multiple calls of close which could occur by calling
            // close -> GameWindow.Close -> GameWindow.Closed event
            if (!_calledClose)
            {
                _calledClose = true;

                TryCloseGameWindow();
                base.Close();

                _renderer?.Dispose();

//            _mouseHandler.Dispose();
//            _keyboardHandler.Dispose();
//            _focusManager.Dispose();
                // todo ? BitmapFactory.Dispose();
                TaskQueue.Dispose();
                Dispose();
                _frameLimiter.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            TryCloseGameWindow();
            base.Dispose(disposing);
        }

        private void TryCloseGameWindow()
        {
            try
            {
                // call to GameWindow.Close(); and afterwards Dispose will
                // create an error under linux (BadWindow from window manager X), 
                // so we just call Dispose which will also close the window.
                GameWindow.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // there seems to be no safe way to check if the window
                // is disposed already so we just catch the ObjectDisposedException
                // even the Disposed event is not executed if the user clicks on the X
                // to close the window...
            }
        }
    }
}
