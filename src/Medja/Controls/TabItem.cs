namespace Medja.Controls
{
	public class TabItem : ContentControl
	{
		public Property<string> PropertyHeader;
		public string Header
		{
			get { return PropertyHeader.Get(); }
			set { PropertyHeader.Set(value); }
		}

		public Property<bool> PropertyIsSelected;
		public bool IsSelected
		{
			get { return PropertyIsSelected.Get(); }
			set { PropertyIsSelected.Set(value); }
		}

		public TabItem()
		{
			PropertyHeader = new Property<string>();
			PropertyIsSelected = new Property<bool>();
		}

		public TabItem(string header, Control content = null)
			: this()
		{
			Header = header;
			Content = content;
		}
	}
}
