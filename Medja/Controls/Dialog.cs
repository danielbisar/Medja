using System;

namespace Medja.Controls
{
	public class Dialog : ContentControl
	{
		public Property<DialogParentControl> PropertyDialogParent;
		public DialogParentControl DialogParent
		{
			get { return PropertyDialogParent.Get(); }
			set { PropertyDialogParent.Set(value); }
		}

		public Dialog()
		{
			PropertyDialogParent = new Property<DialogParentControl>();
		}

		public void Show()
		{
			DialogParent.IsDialogVisible = true;
		}
	}
}
