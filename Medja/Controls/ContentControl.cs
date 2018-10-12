using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	public class ContentControl : Control
	{
		protected readonly ContentArranger ContentArranger;
		
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
			
			ContentArranger = new ContentArranger();
		}

		protected virtual void OnContentChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			var content = eventArgs.OldValue as Control;

			if (content != null && content.Parent == this)
				content.Parent = null;

			content = eventArgs.NewValue as Control;
			ContentArranger.Control = content;
			
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

			var area = Rect.Subtract(Position, Margin);
			area.Subtract(Padding);
			
			ContentArranger.Position(area);
			ContentArranger.Stretch(area);
		}

		public override IEnumerable<Control> GetChildren()
		{
			if (Content != null)
				yield return Content;
		}

		public override void UpdateAnimations()
		{
			base.UpdateAnimations();

			if (Content != null)
				Content.UpdateAnimations();
		}
	}
}
