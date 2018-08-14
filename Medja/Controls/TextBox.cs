using System;

namespace Medja.Controls
{
	public class TextBox : TextControl
	{
		public TextBox()
		{
			InputState.KeyPressed += OnKeyPressed;
		}

		private void OnKeyPressed(object sender, KeyboardEventArgs e)
		{
			if (e.Key == '\b')
			{
				if (Text.Length > 0)
					Text = Text.Substring(0, Text.Length - 1);
			}
			else
				Text += e.Key;
		}
	}
}
