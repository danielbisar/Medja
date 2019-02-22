using Medja.Primitives;

namespace Medja.Controls
{
	public class TextBox : TextControl
	{
		public TextBox()
		{
			PropertyText.UnnotifiedSet(string.Empty);
			PropertyTextWrapping.UnnotifiedSet(TextWrapping.None);
			
			InputState.KeyPressed += OnKeyPressed;
			InputState.OwnsMouseEvents = true;
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
