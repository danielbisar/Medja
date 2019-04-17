using Medja.Controls;
using Medja.Primitives;
using OpenTK;
using OpenTK.Input;

namespace Medja.OpenTk.Input
{
	public class OpenTKKeyboardHandler
	{
		private readonly MedjaWindow _medjaWindow;
		private readonly GameWindow _window;
		private readonly FocusManager _focusManager;

		public OpenTKKeyboardHandler(MedjaWindow medjaWindow, GameWindow window, FocusManager focusManager)
		{
			_medjaWindow = medjaWindow;
			_window = window;
			_focusManager = focusManager;

			_window.KeyPress += OnKeyPressed;
			_window.KeyDown += OnKeyDown;
		}

		private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.BackSpace)
				NotifyKeyPressed('\b');
			else if(e.Key == Key.Delete)
				NotifyKeyPressed((char) Keys.Delete);
			else if(e.Key == Key.Left)
				NotifyKeyPressed((char)Keys.Left);
			else if(e.Key == Key.Right)
				NotifyKeyPressed((char)Keys.Right);
		}

		private void OnKeyPressed(object sender, KeyPressEventArgs e)
		{
			NotifyKeyPressed(e.KeyChar);
		}

		private void NotifyKeyPressed(char c)
		{
			var focusedControl = _focusManager.GetFocused();

			if (focusedControl != null)
			{
				focusedControl.InputState.NotifyKeyPressed(c);
			}
		}
	}
}
