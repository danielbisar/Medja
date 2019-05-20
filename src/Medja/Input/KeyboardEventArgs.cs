using System;

namespace Medja.Input
{
	public class KeyboardEventArgs : EventArgs
	{
		/// <summary>
		/// The unicode code representing the pressed key. 
		/// </summary>
		/// <remarks>This does not contain modifier keys <see cref="get_ModifierKeys"/>. For some special keys there
		/// are predefined codes in <see cref="Keys"/>.</remarks>
		public char KeyChar { get; }
		
		/// <summary>
		/// Is only set if a special key is used (use f.e. for arrow keys)
		/// </summary>
		public Keys? Key { get; }
		
		/// <summary>
		/// Contains the pressed modifier keys (bitwise combined)
		/// </summary>
		public ModifierKeys ModifierKeys { get; }

		public KeyboardEventArgs(char keyChar, ModifierKeys modifierKeys)
		{
			KeyChar = keyChar;
			ModifierKeys = modifierKeys;
		}

		public KeyboardEventArgs(Keys key, ModifierKeys modifierKeys)
			: this((char)key, modifierKeys)
		{
			Key = key;
		}

		public override string ToString()
		{
			return nameof(KeyboardEventArgs) + ": " + nameof(ModifierKeys) + " = " + ModifierKeys.ToString() + ", " +
			       nameof(KeyChar) + " = '" + KeyChar + "' (" + (int) KeyChar + ")";
		}
	}
}
