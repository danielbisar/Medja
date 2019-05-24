namespace Medja.Controls
{
	public class FocusManager
	{
		public static FocusManager Default { get; }

		static FocusManager()
		{
			Default = new FocusManager();
		}
		
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
