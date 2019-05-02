using System;
using Medja.Controls;
using OpenTK;

namespace Medja.OpenTk
{
	public class OpenTkWindow : MedjaWindow
	{
		private int _ignoreNextNResizeEvents;
		private bool _calledClose;

		public GameWindow GameWindow { get; }

		public OpenTkWindow()
		{
			_ignoreNextNResizeEvents = 0;
			GameWindow = new GameWindow();

			GameWindow.Load += OnWindowLoad;
			GameWindow.Resize += OnGameWindowResize;
			GameWindow.Closed += OnGameWindowClosed;

			PropertyTitle.PropertyChanged += (s,e) => GameWindow.Title = e.NewValue as string;
			Position.PropertyX.PropertyChanged += (s,e) => GameWindow.X = (int)Position.X;
			Position.PropertyY.PropertyChanged += (s,e) => GameWindow.Y = (int)Position.Y;
			Position.PropertyWidth.PropertyChanged += (s,e) => GameWindow.Width = (int)Position.Width;
			Position.PropertyHeight.PropertyChanged += (s,e) => GameWindow.Height = (int)Position.Height;
		}

		private void OnWindowLoad(object sender, EventArgs e)
		{
			// OpenTK call OnResize for Width and Height after Load, but the values are not up to date yet, so we just ignore the events
			_ignoreNextNResizeEvents = 2;
		}

		private void OnGameWindowResize(object sender, EventArgs eventArgs)
		{
			if (_ignoreNextNResizeEvents > 0)
			{
				_ignoreNextNResizeEvents--;
				return;
			}

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
