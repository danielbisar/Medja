using System;
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

		public override void Arrange(Size availableSize)
		{
			//base.Arrange(availableSize);
			// the base class would use Position and forward it to its content
			// this doesn't make sense for windows, because their position
			// is relative to the desktop and the controls position relative
			// to the window

			if (Content != null)
			{
				var pos = Content.Position;
				var margin = Content.Margin;

				if (Content.HorizontalAlignment == HorizontalAlignment.Right)
					pos.X = pos.Width - (Padding.Right + margin.Right);
				else
					pos.X = Padding.Left + margin.Left;

				if (Content.VerticalAlignment == VerticalAlignment.Bottom)
					pos.Y = Position.Height - (pos.Height + Padding.Bottom + margin.Bottom);
				else
					pos.Y = Padding.Top + margin.Top;
				
				ArrangeContent();
			}
		}
		
		protected override void ArrangeContent()
		{
			if (Content == null) 
				return;
			
			var pos = Content.Position;

			if(Content.HorizontalAlignment == HorizontalAlignment.Stretch || Content.HorizontalAlignment == HorizontalAlignment.None)
				pos.Width = Position.Width - (Padding.LeftAndRight + Margin.LeftAndRight);

			if (Content.VerticalAlignment == VerticalAlignment.Stretch || Content.VerticalAlignment == VerticalAlignment.None)
				pos.Height = Position.Height - (Padding.TopAndBottom + Margin.TopAndBottom);

			Content.Arrange(new Size(pos.Width, pos.Height));
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
