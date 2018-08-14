using System;

namespace Medja.Controls
{
	public class FocusManager
	{
		private Control _currentlyFocusedControl;

		public void SetFocus(Control control)
		{
			if (control != _currentlyFocusedControl)
			{
				if (_currentlyFocusedControl != null)
					_currentlyFocusedControl.IsFocused = false;

				if (control != null)
					control.IsFocused = true;

				_currentlyFocusedControl = control;
			}
		}

		public Control GetFocused()
		{
			return _currentlyFocusedControl;
		}
	}
}
