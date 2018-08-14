using System;

namespace Medja.Controls
{
	public class KeyboardEventArgs : EventArgs
	{
		public char Key { get; }

		public KeyboardEventArgs(char key)
		{
			Key = key;
		}
	}
}
