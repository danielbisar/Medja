using Medja.Properties;

namespace Medja.Controls.Container
{
	public class TabItem : Control
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

		public readonly Property<Control> PropertyContent;
		public Control Content
		{
			get { return PropertyContent.Get(); }
			set { PropertyContent.Set(value); }
		}

		public TabItem()
		{
			PropertyContent = new Property<Control>();
			PropertyHeader = new Property<string>();
			PropertyIsSelected = new Property<bool>();
		}
	}
}
