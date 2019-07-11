using System;
using Medja.Controls;
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

		public GameWindow GameWindow { get; }

		public OpenTkWindow()
		{
			GameWindow = new GameWindow();

			GameWindow.Resize += OnGameWindowResize;
			GameWindow.Closed += OnGameWindowClosed;

			GameWindow.Title = Title;
            PropertyState.ForwardTo(v => GameWindow.WindowState = GetOpenTkWindowState(v));
			
			PropertyTitle.PropertyChanged += (s,e) => GameWindow.Title = e.NewValue as string;
			Position.PropertyX.PropertyChanged += (s,e) => GameWindow.X = (int)Position.X;
			Position.PropertyY.PropertyChanged += (s,e) => GameWindow.Y = (int)Position.Y;
			Position.PropertyWidth.PropertyChanged += (s,e) => GameWindow.Width = (int)Position.Width;
			Position.PropertyHeight.PropertyChanged += (s,e) => GameWindow.Height = (int)Position.Height;
		}

        private void OnGameWindowResize(object sender, EventArgs eventArgs)
		{
			Position.Width = GameWindow.ClientRectangle.Width;
			Position.Height = GameWindow.ClientRectangle.Height;
		}

		private void OnGameWindowClosed(object sender, EventArgs e)
		{
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
