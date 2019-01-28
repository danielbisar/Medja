namespace Medja.Controls
{
	public class Button : TextControl, IButton
	{
		public readonly Property<bool> PropertyIsSelected;
		public bool IsSelected
		{
			get { return PropertyIsSelected.Get(); }
			set { PropertyIsSelected.Set(value); }
		}

		public Button()
		{
			PropertyIsSelected = new Property<bool>();
			InputState.OwnsMouseEvents = true;
		}
		
		public override string ToString()
		{
			return "Button: " + Text;
		}
	}
}
