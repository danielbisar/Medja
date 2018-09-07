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
			DialogParent.IsDialogVisible = true;
		}
	}
}
