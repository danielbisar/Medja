using System;
using Medja.Controls;
using Medja.Properties;
using OpenTK;
using WindowState = OpenTK.WindowState;

namespace Medja.OpenTk
{
    public class OpenTkWindow : MedjaWindow
    {
        private static WindowState GetOpenTkWindowState(Controls.WindowState windowState)
        {
            switch (windowState)
            {
                case Controls.WindowState.Normal:
                    return WindowState.Normal;
                case Controls.WindowState.Fullscreen:
                    return WindowState.Fullscreen;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowState), windowState, null);
            }
        }
        
        private bool _calledClose;
        private bool _selfUpdatePosition;

        public GameWindow GameWindow { get; }

        public OpenTkWindow()
        {
            GameWindow = new GameWindow();

            GameWindow.Load += OnGameWindowLoad;
            GameWindow.Move += OnGameWindowMove;
            GameWindow.Resize += OnGameWindowResize;
            GameWindow.Closed += OnGameWindowClosed;

            GameWindow.Title = Title;
            Position.X = GameWindow.X;
            Position.Y = GameWindow.Y;
            Position.Height = GameWindow.Height;
            Position.Width = GameWindow.Width;
            
            PropertyState.ForwardTo(v => GameWindow.WindowState = GetOpenTkWindowState(v));
            
            PropertyTitle.PropertyChanged += (s,e) => GameWindow.Title = e.NewValue as string;
            Position.PropertyX.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyY.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyWidth.PropertyChanged += OnPositionPropertyChanged;
            Position.PropertyHeight.PropertyChanged += OnPositionPropertyChanged;
        }

        private void OnGameWindowLoad(object sender, EventArgs e)
        {
            Console.WriteLine("Load");
        }

        private void OnPositionPropertyChanged(object sender, PropertyChangedEventArgs eventargs)
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
            GameWindow.Run();
        }

        private void OnGameWindowResize(object sender, EventArgs eventArgs)
        {
            _selfUpdatePosition = true;
            Position.Width = GameWindow.Width;
            Position.Height = GameWindow.Height;
            _selfUpdatePosition = false;
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
