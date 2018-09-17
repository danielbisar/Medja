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
			if (DialogParent != null)
				DialogParent.IsDialogVisible = true;
		}
		
		public void Hide()
		{
			if (DialogParent != null)
				DialogParent.IsDialogVisible = false;
		}
	}
}
