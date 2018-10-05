using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	public class ContentControl : Control
	{
		public readonly Property<Control> PropertyContent;
		public Control Content
		{
			get { return PropertyContent.Get(); }
			set { PropertyContent.Set(value); }
		}

		public Thickness Padding { get; set; }

		public ContentControl()
		{
			PropertyContent = new Property<Control>();
			PropertyContent.PropertyChanged += OnContentChanged;
			Padding = new Thickness();
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
		}

		protected virtual void OnContentChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			var content = eventArgs.OldValue as Control;

			if (content != null && content.Parent == this)
				content.Parent = null;

			content = eventArgs.NewValue as Control;
			
			if (content != null)
				content.Parent = this;
		}

		private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (Content != null)
				Content.IsEnabled = (bool)eventArgs.NewValue;
		}

		public override Size Measure(Size availableSize)
		{
			if (Content == null)
				return base.Measure(availableSize);

			return Content.Measure(availableSize);
		}

		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);

			if (Content != null)
			{
				var pos = Content.Position;
				var margin = Content.Margin;

				if (Content.HorizontalAlignment == HorizontalAlignment.Right)
					pos.X = Position.X + pos.Width - (Padding.Right + margin.Right);
				else
					pos.X = Position.X + (Padding.Left + margin.Left);

				if (Content.VerticalAlignment == VerticalAlignment.Bottom)
					pos.Y = Position.Y + pos.Height - (Padding.Bottom + margin.Bottom);
				else
					pos.Y = Position.Y + (Padding.Top + margin.Top);
				
				ArrangeContent();
			}
		}

		protected virtual void ArrangeContent()
		{
			if (Content == null) 
				return;
			
			var pos = Content.Position;

			if(Content.HorizontalAlignment != HorizontalAlignment.Left 
					&& Content.VerticalAlignment != VerticalAlignment.Bottom)
				pos.Width = Position.Width - (Padding.LeftAndRight + Margin.LeftAndRight);

			if (Content.VerticalAlignment != VerticalAlignment.Top &&
					Content.VerticalAlignment != VerticalAlignment.Bottom)
				pos.Height = Position.Height - (Padding.TopAndBottom + Margin.TopAndBottom);

			Content.Arrange(new Size(pos.Width, pos.Height));
		}

		public override IEnumerable<Control> GetAllControls()
		{
			yield return this;

			if (Content != null)
			{
				foreach (var control in Content.GetAllControls())
					yield return control;
			}
		}

		public override void UpdateAnimations()
		{
			base.UpdateAnimations();

			if (Content != null)
				Content.UpdateAnimations();
		}
	}
}
