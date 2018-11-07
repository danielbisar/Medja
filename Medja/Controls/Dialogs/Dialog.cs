using System.Runtime.InteropServices;

namespace Medja.Controls
{
	public class Dialog : ContentControl
	{
		public DialogParentControl DialogParent
		{
			get { return Parent as DialogParentControl; }
			set { Parent = value; }
		}

		public Dialog()
		{
		}

		public void Show()
		{
			DialogService.Show(this);
		}
		
		public void Hide()
		{
			DialogService.Hide(this);
		}
	}
}
