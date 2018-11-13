using System;
using System.Collections.Generic;
using Medja.Debug;
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
			PropertyContent.AffectsLayout(this);
			
			Padding = new Thickness();
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
			PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;
			
			ContentArranger = new ContentArranger();
		}

		protected virtual void OnIsLayoutUpdatedChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Content != null)
				Content.IsLayoutUpdated = (bool)e.NewValue;
		}

		protected virtual void OnContentChanged(object sender, PropertyChangedEventArgs e)
		{
			var content = e.OldValue as Control;

			if (content != null && content.Parent == this)
				content.Parent = null;

			content = e.NewValue as Control;
			ContentArranger.Control = content;
			
			if (content != null)
				content.Parent = this;
		}

		private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (Content != null)
			{
				if(IsEnabled)
					Content.PropertyIsEnabled.ClearOverwrittenValue();
				else
					Content.PropertyIsEnabled.OverwriteSet(false);
			}
		}

		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);

			var area = new Rect(Position.X, Position.Y, availableSize.Width, availableSize.Height);
			area.Subtract(Margin);
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
