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

			TitleProperty.PropertyChanged += p => GameWindow.Title = Title;
			Position.PropertyX.PropertyChanged += p => GameWindow.X = (int)Position.X;
			Position.PropertyY.PropertyChanged += p => GameWindow.Y = (int)Position.Y;
			Position.PropertyWidth.PropertyChanged += p => GameWindow.Width = (int)Position.Width;
			Position.PropertyHeight.PropertyChanged += p => GameWindow.Height = (int)Position.Height;
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

			Position.PropertyWidth.UnnotifiedSet(GameWindow.ClientRectangle.Width);
			Position.PropertyHeight.UnnotifiedSet(GameWindow.ClientRectangle.Height);
		}

		private void OnGameWindowClosed(object sender, EventArgs e)
		{
			Close();
		}

		public override void Close()
		{
			// prevent multiple calls of close which could occure by calling
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
				GameWindow.Close();
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
