namespace Medja.Controls.Buttons
{
	public class Button : TextControl
	{
		public Button()
		{
			InputState.OwnsMouseEvents = true;
		}

		public override string ToString()
		{
			return "Button: " + Text;
		}
	}
}
