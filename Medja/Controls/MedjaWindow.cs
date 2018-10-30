using System;
using Medja.Debug;
using Medja.Primitives;

namespace Medja.Controls
{
	public class MedjaWindow : ContentControl
	{
		public readonly Property<string> PropertyTitle;
		public string Title
		{
			get { return PropertyTitle.Get(); }
			set { PropertyTitle.Set(value); }
		}

		public bool IsClosed { get; set; }

		public event EventHandler Closed;

		public MedjaWindow()
		{
			PropertyTitle = new Property<string>();
		}
		
		protected override void OnContentChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnContentChanged(sender, e);

			var content = e.NewValue as Control;

			if (content == null)
				return;

			if (content.VerticalAlignment == VerticalAlignment.None)
				content.VerticalAlignment = VerticalAlignment.Stretch;
				
			if (content.HorizontalAlignment == HorizontalAlignment.None)
				content.HorizontalAlignment = HorizontalAlignment.Stretch;
		}

		public override void Arrange(Size availableSize)
		{
			//base.Arrange(availableSize);
			// the base class would use Position and forward it to its content
			// this doesn't make sense for windows, because their position
			// is relative to the desktop and the controls position relative
			// to the window
			
			var area = new Rect(0, 0, availableSize.Width, availableSize.Height);
			area.Subtract(Margin);
			area.Subtract(Padding);
			
			ContentArranger.Position(area);
			ContentArranger.Stretch(area);
		}

		public virtual void Close()
		{
			IsClosed = true;
			NotifyClosed();
		}

		private void NotifyClosed()
		{
			if (Closed != null)
				Closed(this, EventArgs.Empty);
		}
	}
}
