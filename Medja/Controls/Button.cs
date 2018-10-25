namespace Medja.Controls
{
	public class Button : TextControl
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
		}
		
		public override string ToString()
		{
			return "Button: " + Text;
		}
	}
}
