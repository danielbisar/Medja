using System;

namespace Medja.Controls
{
	public class Dialog : ContentControl
	{
		public DialogParentControl DialogParent
		{
			get { return Parent as DialogParentControl; }
			set { Parent = value; }
		}

		public event EventHandler Closed;
		
		public Dialog()
		{
		}

		public void Show()
		{
			DialogService.Show(this);
			NeedsRendering = true;
		}
		
		public void Hide()
		{
			DialogService.Hide(this);
			Closed?.Invoke(this, EventArgs.Empty);
		}
	}
}
