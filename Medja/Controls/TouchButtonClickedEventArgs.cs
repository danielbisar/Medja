using System;
namespace Medja.Controls
{
	public class TouchButtonClickedEventArgs : EventArgs
	{
		public Button Button { get; set; }
		public object Item { get; set; }

		public TouchButtonClickedEventArgs(Button button, object item)
		{
			Button = button;
			Item = item;
		}
	}
}
