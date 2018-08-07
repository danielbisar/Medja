using System;
using Medja.Controls;
using OpenTK;

namespace Medja.OpenTk
{
	public class OpenTkWindow : MedjaWindow
	{
		private int _ignoreNextNResizeEvents;
		private bool _isGameWindowDisposed;

		public GameWindow GameWindow { get; }

		public OpenTkWindow()
		{
			_ignoreNextNResizeEvents = 0;
			GameWindow = new GameWindow();

			GameWindow.Disposed += OnGameWindowDisposed;
			GameWindow.Load += OnWindowLoad;
			GameWindow.Resize += OnGameWindowResize;

			TitleProperty.PropertyChanged += p => GameWindow.Title = Title;
			Position.PropertyX.PropertyChanged += p => GameWindow.X = (int)Position.X;
			Position.PropertyY.PropertyChanged += p => GameWindow.Y = (int)Position.Y;
			Position.PropertyWidth.PropertyChanged += p => GameWindow.Width = (int)Position.Width;
			Position.PropertyHeight.PropertyChanged += p => GameWindow.Height = (int)Position.Height;
		}

		private void OnGameWindowDisposed(object sender, EventArgs e)
		{
			_isGameWindowDisposed = true;
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

		public override void Close()
		{
			if (!_isGameWindowDisposed)
				GameWindow.Close();

			base.Close();
		}
	}
}
